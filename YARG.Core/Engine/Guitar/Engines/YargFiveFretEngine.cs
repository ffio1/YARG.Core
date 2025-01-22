using System;
using YARG.Core.Chart;
using YARG.Core.Input;
using YARG.Core.Logging;

namespace YARG.Core.Engine.Guitar.Engines
{
    public class YargFiveFretEngine : GuitarEngine
    {
        public bool IsGamepadMode { get; private set; }

        /// <summary>
        /// For gamepad mode, the amount of time you have between hitting one fret of a chord and the other(s).
        /// Hitting a chord ends it, and if it expires you overstrum.
        /// StrumLeniencyTimer is used for this first, and *then* ChordLeniencyTimer. It makes sense, trust me.
        /// </summary>
        protected EngineTimer GamepadModeChordLeniencyTimer;

        protected EngineTimer GamepadModeLiftedNotePressLeniencyTimer;
        private GuitarNote? GamepadModeLiftedNotePressLeniencyNote;

        private int GamepadModePressedSustainsMask;

        public YargFiveFretEngine(InstrumentDifficulty<GuitarNote> chart, SyncTrack syncTrack,
            GuitarEngineParameters engineParameters, bool isBot, bool isGamepadMode)
            : base(chart, syncTrack, engineParameters, isBot)
        {
            IsGamepadMode = isBot ? false : isGamepadMode; // No gamepad mode for the bot just in case, since the bot having gamepad mode and knowing how to use it is identical to the bot not having gamepad mode
            GamepadModeChordLeniencyTimer = new EngineTimer(engineParameters.GamepadModeChordLeniency);
            GamepadModeLiftedNotePressLeniencyTimer = new EngineTimer(0);
            GamepadModePressedSustainsMask = ButtonMask;
        }

        protected override void UpdateBot(double time)
        {
            if (!IsBot || NoteIndex >= Notes.Count)
            {
                return;
            }

            var note = Notes[NoteIndex];

            if (time < note.Time)
            {
                return;
            }

            LastButtonMask = ButtonMask;
            ButtonMask = (byte) note.NoteMask;

            YargLogger.LogFormatTrace("[Bot] Set button mask to: {0}", ButtonMask);

            HasTapped = ButtonMask != LastButtonMask;
            IsFretPress = true;
            HasStrummed = false;
            StrumLeniencyTimer.Start(time);

            foreach (var sustain in ActiveSustains)
            {
                var sustainNote = sustain.Note;

                if (!sustainNote.IsExtendedSustain)
                {
                    continue;
                }

                if (sustainNote.IsDisjoint)
                {
                    ButtonMask |= (byte) sustainNote.DisjointMask;

                    YargLogger.LogFormatTrace("[Bot] Added Disjoint Sustain Mask {0} to button mask. {1}", sustainNote.DisjointMask, ButtonMask);
                }
                else
                {
                    ButtonMask |= (byte) sustainNote.NoteMask;

                    YargLogger.LogFormatTrace("[Bot] Added Sustain Mask {0} to button mask. {1}", sustainNote.NoteMask, ButtonMask);
                }
            }
        }

        protected override void MutateStateWithInput(GameInput gameInput)
        {
            var action = gameInput.GetAction<GuitarAction>();

            // Star power
            if (action is GuitarAction.StarPower)
            {
                IsStarPowerInputActive = gameInput.Button;
            }
            else if (action is GuitarAction.Whammy)
            {
                LastWhammyTimerState = StarPowerWhammyTimer.IsActive;
                StarPowerWhammyTimer.Start(gameInput.Time);
            }
            else if (action is GuitarAction.StrumDown or GuitarAction.StrumUp && gameInput.Button && !IsGamepadMode)
            {
                HasStrummed = true;
            }
            else if (IsFretInput(gameInput))
            {
                LastButtonMask = ButtonMask;
                HasFretted = true;
                IsFretPress = gameInput.Button;

                ToggleFret(gameInput.Action, gameInput.Button);

                // No other frets are held, enable the "open fret"
                // Always enable the "open fret" on gamepad mode
                if ((ButtonMask & ~OPEN_MASK) == 0)
                {
                    ButtonMask |= OPEN_MASK;
                }
                else
                {
                    // Some frets are held, disable the "open fret"
                    ButtonMask &= unchecked((byte) ~OPEN_MASK);
                }

                if (IsGamepadMode)
                {
                    if (IsFretPress) HasStrummed = true;
                    else if (!IsFretPress && EngineParameters.GamepadModeStrumOnRelease) {
                        HasStrummed = true;
                            
                        // We don't want to strum on release if we're releasing a fret that's part of an active sustain
                        var droppedMask = LastButtonMask & ~ButtonMask;
                        if ((droppedMask & GamepadModePressedSustainsMask) != 0) {
                            HasStrummed = false;
                            GamepadModePressedSustainsMask &= ~droppedMask;
                        }
                    }
                }
                
            }

            YargLogger.LogFormatTrace("Mutated input state: Button Mask: {0}, HasFretted: {1}, HasStrummed: {2}",
                ButtonMask, HasFretted, HasStrummed);
        }

        protected override void UpdateHitLogic(double time)
        {
            UpdateTimers();

            bool strumEatenByHopo = false;

            // This is up here so overstrumming still works when there are no notes left
            if (HasStrummed)
            {
                // Hopo was hit recently, eat strum input
                if (HopoLeniencyTimer.IsActive)
                {
                    StrumLeniencyTimer.Disable();

                    // Disable hopo leniency as hopos can only eat one strum
                    HopoLeniencyTimer.Disable();

                    strumEatenByHopo = true;
                    ReRunHitLogic = true;
                }
                else if (IsFretPress && GamepadModeLiftedNotePressLeniencyTimer.IsActive && CanNoteBeHit(GamepadModeLiftedNotePressLeniencyNote!)) // Note won't be null if Timer is active
                {
                    StrumLeniencyTimer.Disable();
                    GamepadModeLiftedNotePressLeniencyTimer.Disable();
                    GamepadModeChordLeniencyTimer.Disable();

                    strumEatenByHopo = true;
                    ReRunHitLogic = true;
                }
                else
                {
                    // Strummed while strum leniency is active (double strum)
                    if (StrumLeniencyTimer.IsActive && !(IsGamepadMode && HasFretted && !IsFretPress))
                    {
                        if (IsGamepadMode) GamepadModeChordLeniencyTimer.Start(CurrentTime);
                        else Overstrum();
                    }
                }

                if (!strumEatenByHopo && !(IsGamepadMode && HasFretted && !IsFretPress))
                {
                    double offset = 0;

                    if (NoteIndex >= Notes.Count || !IsNoteInWindow(Notes[NoteIndex]))
                    {
                        offset = EngineParameters.StrumLeniencySmall;
                    }

                    StartTimer(ref StrumLeniencyTimer, CurrentTime, offset);

                    ReRunHitLogic = true;
                }
            }

            // Update bot (will return if not enabled)
            UpdateBot(time);

            // Quit early if there are no notes left
            if (NoteIndex >= Notes.Count)
            {
                HasStrummed = false;
                HasFretted = false;
                IsFretPress = false;
                UpdateSustains();
                return;
            }

            var note = Notes[NoteIndex];

            var hitWindow = EngineParameters.HitWindow.CalculateHitWindow(GetAverageNoteDistance(note));
            var frontEnd = EngineParameters.HitWindow.GetFrontEnd(hitWindow);

            if (HasFretted)
            {
                HasTapped = true;

                // This is the time the front end will expire. Used for hit logic with infinite front end
                FrontEndExpireTime = CurrentTime + Math.Abs(frontEnd);

                // Check for fret ghosting
                // We want to run ghost logic regardless of the setting for the ghost counter
                bool ghosted = CheckForGhostInput(note);

                // This variable controls hit logic for ghosting
                // Anti-ghosting is disabled in Gamepad Mode since it inherently has anti-ghosting (you would overstrum because every press is a strum)
                WasNoteGhosted = EngineParameters.AntiGhosting && !IsGamepadMode && (ghosted || WasNoteGhosted);

                // Add ghost inputs to stats regardless of the setting for anti ghosting
                if (ghosted)
                {
                    EngineStats.GhostInputs++;
                }
            }

            CheckForNoteHit();
            UpdateSustains();

            HasStrummed = false;
            HasFretted = false;
            IsFretPress = false;
        }

        protected override void CheckForNoteHit()
        {
            for (int i = NoteIndex; i < Notes.Count; i++)
            {
                bool isFirstNoteInWindow = i == NoteIndex;
                var note = Notes[i];

                if (note.WasFullyHitOrMissed())
                {
                    break;
                }

                if (!IsNoteInWindow(note, out bool missed))
                {
                    if (isFirstNoteInWindow && missed)
                    {
                        MissNote(note);
                        YargLogger.LogFormatTrace("Missed note (Index: {0}, Mask: {1}) at {2}", i,
                            note.NoteMask, CurrentTime);
                    }

                    break;
                }

                // Cannot hit the note
                if (!CanNoteBeHit(note))
                {
                    YargLogger.LogFormatTrace("Cant hit note (Index: {0}, Mask {1}) at {2}. Buttons: {3}", i,
                        note.NoteMask, CurrentTime, ButtonMask);
                    // This does nothing special, it's just logging strum leniency
                    if (isFirstNoteInWindow && HasStrummed && StrumLeniencyTimer.IsActive)
                    {
                        YargLogger.LogFormatTrace("Starting strum leniency at {0}, will end at {1}", CurrentTime,
                            StrumLeniencyTimer.EndTime);
                    }

                    // Note skipping not allowed on the first note if hopo/tap
                    if ((note.IsHopo || note.IsTap) && NoteIndex == 0)
                    {
                        break;
                    }

                    // Continue to the next note (skipping the current one)
                    continue;
                }

                // Handles hitting a hopo notes
                // If first note is a hopo then it can be hit without combo (for practice mode)
                bool hopoCondition = note.IsHopo && isFirstNoteInWindow &&
                    (EngineStats.Combo > 0 || NoteIndex == 0);

                // If a note is a tap then it can be hit only if it is the closest note, unless
                // the combo is 0 then it can be hit regardless of the distance (note skipping)
                bool tapCondition = note.IsTap && (isFirstNoteInWindow || EngineStats.Combo == 0);

                bool frontEndIsExpired = note.Time > FrontEndExpireTime;
                bool canUseInfFrontEnd =
                    EngineParameters.InfiniteFrontEnd || !frontEndIsExpired || NoteIndex == 0;

                // Attempt to hit with hopo/tap rules
                if (HasTapped && (hopoCondition || tapCondition) && canUseInfFrontEnd && !WasNoteGhosted)
                {
                    HitNote(note);
                    YargLogger.LogFormatTrace("Hit note (Index: {0}, Mask: {1}) at {2} with hopo rules",
                        i, note.NoteMask, CurrentTime);
                    break;
                }

                // If hopo/tap checks failed then the note can be hit if it was strummed
                if ((HasStrummed || StrumLeniencyTimer.IsActive) &&
                    (isFirstNoteInWindow || (NoteIndex > 0 && EngineStats.Combo == 0)))
                {
                    HitNote(note);
                    if (HasStrummed)
                    {
                        YargLogger.LogFormatTrace("Hit note (Index: {0}, Mask: {1}) at {2} with strum input",
                            i, note.NoteMask, CurrentTime);
                    }
                    else
                    {
                        YargLogger.LogFormatTrace("Hit note (Index: {0}, Mask: {1}) at {2} with strum leniency",
                            i, note.NoteMask, CurrentTime);
                    }

                    break;
                }
            }
        }

        protected override void GenerateQueuedUpdates(double nextTime)
        {
            base.GenerateQueuedUpdates(nextTime);
            if (GamepadModeChordLeniencyTimer.IsActive)
            {
                if (IsTimeBetween(GamepadModeChordLeniencyTimer.EndTime, CurrentTime, nextTime))
                {
                    YargLogger.LogFormatTrace("Queuing gamepad mode chord leniency end time at {0}",
                        GamepadModeChordLeniencyTimer.EndTime);
                    QueueUpdateTime(GamepadModeChordLeniencyTimer.EndTime, "Gamepad Mode Chord Leniency End");
                }
            }
            if (GamepadModeLiftedNotePressLeniencyTimer.IsActive)
            {
                if (IsTimeBetween(GamepadModeLiftedNotePressLeniencyTimer.EndTime, CurrentTime, nextTime))
                {
                    YargLogger.LogFormatTrace("Queuing gamepad mode lifted note press leniency end time at {0}",
                        GamepadModeLiftedNotePressLeniencyTimer.EndTime);
                    QueueUpdateTime(GamepadModeLiftedNotePressLeniencyTimer.EndTime, "Gamepad Mode Lifted Note Press Leniency End");
                }
            }
        }

        public override void Reset(bool keepCurrentButtons = false)
        {
            base.Reset(keepCurrentButtons);
            GamepadModeChordLeniencyTimer.Disable();
            GamepadModeLiftedNotePressLeniencyTimer.Disable();
            GamepadModePressedSustainsMask = 0;
        }

        public override void SetSpeed(double speed)
        {
            base.SetSpeed(speed);
            GamepadModeChordLeniencyTimer.SetSpeed(speed);
            GamepadModeLiftedNotePressLeniencyTimer.SetSpeed(speed);
        }

        protected override bool CanNoteBeHit(GuitarNote note)
        {
            // In gamepad mode, on a release, we use LastButtonMask instead of ButtonMask.
            // This is because, if you're *releasing*, then the fret that the note you want to hit is on *isn't actually being held*, because, well, you released it.
            // But you should still be able to hit it -- that's the whole point.
            byte originalButtonMask = (IsGamepadMode && HasFretted && !IsFretPress) ? LastButtonMask : ButtonMask;

            byte buttonsMasked = originalButtonMask;
            if (ActiveSustains.Count > 0)
            {
                foreach (var sustain in ActiveSustains)
                {
                    var sustainNote = sustain.Note;

                    if (sustainNote.IsExtendedSustain)
                    {
                        // Remove the note mask if its an extended sustain
                        // Difference between NoteMask and DisjointMask is that DisjointMask is only a single fret
                        // while NoteMask is the entire chord

                        // TODO Notes cannot be hit if a sustain of the same fret is being held e.g H-ELL Solo 3C5

                        //byte sameFretsHeld = (byte) ((byte) (sustain.Note.NoteMask & note.NoteMask) & ButtonMask);

                        var maskToRemove = sustainNote.IsDisjoint ? sustainNote.DisjointMask : sustainNote.NoteMask;
                        buttonsMasked &= unchecked((byte) ~maskToRemove);
                        //buttonsMasked |= sameFretsHeld;
                    }
                }

                // If the resulting masked buttons are 0, we need to apply the Open Mask so open notes can be hit
                if (buttonsMasked == 0)
                {
                    buttonsMasked |= OPEN_MASK;
                }
            }

            // Gamepad Mode open note handling
            if (IsGamepadMode && (note.NoteMask & OPEN_MASK) != 0) {
                var maskWithoutOpen = note.NoteMask & ~OPEN_MASK;

                // Normal open note (allow hitting by pressing any fret)
                if (maskWithoutOpen == 0) originalButtonMask = (byte) note.NoteMask;
                // Open chord (only allow hitting by pressing the exact frets needed, like open chords should)
                else if (maskWithoutOpen == originalButtonMask || maskWithoutOpen == buttonsMasked)
                {
                    originalButtonMask = (byte) note.NoteMask;
                    buttonsMasked = originalButtonMask;
                } 
            }

            // We dont want to use masked buttons for hit logic if the buttons are identical
            if (ActiveSustains.Count > 0 && buttonsMasked != (buttonsMasked == OPEN_MASK ? originalButtonMask | OPEN_MASK : originalButtonMask) && IsNoteHittable(note, buttonsMasked)) return true;

            // If masked/extended sustain logic didn't work, try original ButtonMask
            return IsNoteHittable(note, originalButtonMask);

            static bool IsNoteHittable(GuitarNote note, byte buttonsMasked)
            {
                // Only used for sustain logic
                bool useDisjointSustainMask = note is { IsDisjoint: true, WasHit: true };

                // Use the DisjointMask for comparison if disjointed and was hit (for sustain logic)
                int noteMask = useDisjointSustainMask ? note.DisjointMask : note.NoteMask;

                // If disjointed and is sustain logic (was hit), can hit if disjoint mask matches
                if (useDisjointSustainMask && (note.DisjointMask & buttonsMasked) != 0)
                {
                    if ((note.DisjointMask & buttonsMasked) != 0)
                    {
                        return true;
                    }

                    if ((note.NoteMask & OPEN_MASK) != 0)
                    {
                        return true;
                    }
                }

                // Open chords
                // Contains open fret but the note mask is not strictly the open mask
                if ((noteMask & OPEN_MASK) != 0 && noteMask != OPEN_MASK)
                {
                    // Open chords are basically normal chords except no anchoring in any circumstances
                    // Prevents HOPO/Tap chords from being anchored

                    var buttonsMaskedWithOpen = buttonsMasked | OPEN_MASK;

                    if (buttonsMaskedWithOpen == noteMask)
                    {
                        return true;
                    }
                }

                // If holding exact note mask, can hit
                if (buttonsMasked == noteMask)
                {
                    return true;
                }

                // Anchoring

                // XORing the two masks will give the anchor (held frets) around the note.
                int anchorButtons = buttonsMasked ^ noteMask;

                // Chord logic
                if (note.IsChord)
                {
                    if (note.IsStrum)
                    {
                        // Buttons must match note mask exactly for strum chords
                        return buttonsMasked == noteMask;
                    }

                    // Anchoring hopo/tap chords

                    // Gets the lowest fret of the chord.
                    var chordMask = 0;
                    for (var fret = GuitarAction.GreenFret; fret <= GuitarAction.OrangeFret; fret++)
                    {
                        chordMask = 1 << (int) fret;

                        // If the current fret mask is part of the chord, break
                        if ((chordMask & note.NoteMask) == chordMask)
                        {
                            break;
                        }
                    }

                    // Anchor part:
                    // Lowest fret of chord must be bigger or equal to anchor buttons
                    // (can't hold note higher than the highest fret of chord)

                    // Button mask subtract the anchor must equal chord mask (all frets of chord held)
                    return chordMask >= anchorButtons && buttonsMasked - anchorButtons == note.NoteMask;
                }

                // Anchoring single notes
                // Anchors are buttons held lower than the note mask

                // Remove the open mask from note otherwise this will always pass (as its higher than all notes)
                // This is only used for single notes, open chords are handled above
                return anchorButtons < (noteMask & unchecked((byte) ~OPEN_MASK));
            }
        }

        protected override void HitNote(GuitarNote note)
        {
            if (note.IsHopo || note.IsTap)
            {
                HasTapped = false;
                StartTimer(ref HopoLeniencyTimer, CurrentTime);
            }
            else
            {
                // This line allows for hopos/taps to be hit using infinite front end after strumming
                HasTapped = true;

                // Does the same thing but ensures it still works when infinite front end is disabled
                EngineTimer.Reset(ref FrontEndExpireTime);
            }

            StrumLeniencyTimer.Disable();

            // Gamepad Mode
            if (note.IsChord) GamepadModeChordLeniencyTimer.Disable();
            if (IsGamepadMode && HasFretted)
            {
                if (!IsFretPress)
                {
                    // Give Lifted Note Press Leniency until the end of the note's hit window
                    GamepadModeLiftedNotePressLeniencyNote = note;
                    GamepadModeLiftedNotePressLeniencyTimer.Start(note.Time + EngineParameters.HitWindow.GetBackEnd(EngineParameters.HitWindow.CalculateHitWindow(GetAverageNoteDistance(note))));
                }
                else if (note.IsSustain && note.TickLength > 1) // implying && IsFretPress -- this should not trigger on release
                {
                    GamepadModePressedSustainsMask |= note.IsDisjoint ? note.DisjointMask : note.NoteMask;
                }
            }

            for(int i = 0; i < ActiveSustains.Count; i++)
            {
                var sustainNote = ActiveSustains[i].Note;

                var sustainMask = sustainNote.IsDisjoint ? sustainNote.DisjointMask : sustainNote.NoteMask;
                if ((sustainMask & note.NoteMask) != 0)
                {
                    EndSustain(i, true, CurrentTick >= sustainNote.TickEnd);
                }
            }

            base.HitNote(note);
        }

        protected override void MissNote(GuitarNote note)
        {
            HasTapped = false;
            base.MissNote(note);
        }

        protected virtual void UpdateTimers()
        {
            if (HopoLeniencyTimer.IsActive && HopoLeniencyTimer.IsExpired(CurrentTime))
            {
                HopoLeniencyTimer.Disable();
                ReRunHitLogic = true;
            }

            if (GamepadModeLiftedNotePressLeniencyTimer.IsActive && GamepadModeLiftedNotePressLeniencyTimer.IsExpired(CurrentTime))
            {
                GamepadModeLiftedNotePressLeniencyTimer.Disable();
                ReRunHitLogic = true;
            }

            if (StrumLeniencyTimer.IsActive)
            {
                //YargTrace.LogInfo("Strum Leniency: Enabled");
                if (StrumLeniencyTimer.IsExpired(CurrentTime))
                {
                    //YargTrace.LogInfo("Strum Leniency: Expired. Overstrumming");
                    if (!IsGamepadMode)
                    {
                        Overstrum();
                        StrumLeniencyTimer.Disable();
                        ReRunHitLogic = true;
                    }
                    else
                    {
                        GamepadModeChordLeniencyTimer.Start(CurrentTime);
                        StrumLeniencyTimer.Disable();
                    }
                }
            }

            if (GamepadModeChordLeniencyTimer.IsActive && GamepadModeChordLeniencyTimer.IsExpired(CurrentTime)) {
                Overstrum();
                GamepadModeChordLeniencyTimer.Disable();
                ReRunHitLogic = true;
            }
        }

        protected bool CheckForGhostInput(GuitarNote note)
        {
            // First note cannot be ghosted, nor can a note be ghosted if a button is unpressed (pulloff)
            if (note.PreviousNote is null || !IsFretPress)
            {
                return false;
            }

            // Note can only be ghosted if it's in timing window
            if (!IsNoteInWindow(note))
            {
                return false;
            }

            // Input is a hammer-on if the highest fret held is higher than the highest fret of the previous mask
            bool isHammerOn = GetMostSignificantBit(ButtonMask) > GetMostSignificantBit(LastButtonMask);

            // Input is a hammer-on and the button pressed is not part of the note mask (incorrect fret)
            if (isHammerOn && (ButtonMask & note.NoteMask) == 0)
            {
                return true;
            }

            return false;
        }

        private static int GetMostSignificantBit(int mask)
        {
            // Gets the most significant bit of the mask
            var msbIndex = 0;
            while (mask != 0)
            {
                mask >>= 1;
                msbIndex++;
            }

            return msbIndex;
        }
    }
}
