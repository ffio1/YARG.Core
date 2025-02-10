using System;
using System.Collections.Generic;
using MoonscraperChartEditor.Song;

namespace YARG.Core.Chart
{
    internal partial class MoonSongLoader : ISongLoader
    {
        public InstrumentTrack<EliteKeysNote> LoadEliteKeysTrack(Instrument instrument)
        {
            return LoadEliteKeysTrack(instrument, CreateEliteKeysNote);
        }

        private InstrumentTrack<EliteKeysNote> LoadEliteKeysTrack(Instrument instrument, CreateNoteDelegate<EliteKeysNote> createNote)
        {
            if (instrument.ToGameMode() != GameMode.EliteKeys)
                throw new ArgumentException($"Instrument {instrument} is not a elite-keys instrument!", nameof(instrument));

            var difficulties = new Dictionary<Difficulty, InstrumentDifficulty<EliteKeysNote>>
            {
                { Difficulty.Easy,   LoadDifficulty(instrument, Difficulty.Easy, createNote) },
                { Difficulty.Medium, LoadDifficulty(instrument, Difficulty.Medium, createNote) },
                { Difficulty.Hard,   LoadDifficulty(instrument, Difficulty.Hard, createNote) },
                { Difficulty.Expert, LoadDifficulty(instrument, Difficulty.Expert, createNote) },
            };
            return new(instrument, difficulties);
        }

        private EliteKeysNote CreateEliteKeysNote(MoonNote moonNote, Dictionary<MoonPhrase.Type, MoonPhrase> currentPhrases)
        {
            var key = moonNote.proKeysKey;
            var generalFlags = GetGeneralFlags(moonNote, currentPhrases);
            var eliteKeysFlags = GetEliteKeysNoteFlags(moonNote, currentPhrases);

            double time = _moonSong.TickToTime(moonNote.tick);
            return new EliteKeysNote(key, eliteKeysFlags, generalFlags, time, GetLengthInTime(moonNote),
                moonNote.tick, moonNote.length);
        }

        private EliteKeysNoteFlags GetEliteKeysNoteFlags(MoonNote moonNote, Dictionary<MoonPhrase.Type, MoonPhrase> currentPhrases)
        {
            var flags = EliteKeysNoteFlags.None;

            if (currentPhrases.TryGetValue(MoonPhrase.Type.EliteKeys_Glissando, out var glissando) &&
                IsEventInPhrase(moonNote, glissando))
            {
                flags |= EliteKeysNoteFlags.Glissando;
            }

            return flags;
        }
    }
}
