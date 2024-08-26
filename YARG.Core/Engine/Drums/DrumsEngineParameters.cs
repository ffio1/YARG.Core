﻿using YARG.Core.Replays.Serialization;

namespace YARG.Core.Engine.Drums
{
    public class DrumsEngineParameters : BaseEngineParameters
    {
        public enum DrumMode : byte
        {
            NonProFourLane,
            ProFourLane,
            FiveLane
        }

        /// <summary>
        /// What mode the inputs should be processed in.
        /// </summary>
        public readonly DrumMode Mode;

        /// <summary>
        /// Velocity threshold for Drum note types.
        /// </summary>
        /// <remarks>
        /// Ghost notes are below the threshold, Accent notes are above the threshold.
        /// </remarks>
        public readonly float VelocityThreshold;

        /// <summary>
        /// The maximum allowed time in seconds between notes to use context-sensitive velocity scoring.
        /// </summary>
        public readonly float SituationalVelocityWindow;

        internal DrumsEngineParameters(SerializedDrumsEngineParameters drumsParams,
            SerializedBaseEngineParameters baseParams) : base(baseParams)
        {
            Mode = drumsParams.Mode;
            VelocityThreshold = drumsParams.VelocityThreshold;
            SituationalVelocityWindow = drumsParams.SituationalVelocityWindow;
        }

        public DrumsEngineParameters(HitWindowSettings hitWindow, int maxMultiplier, float[] starMultiplierThresholds,
            DrumMode mode)
            : base(hitWindow, maxMultiplier, 0, 0, starMultiplierThresholds)
        {
            Mode = mode;
            VelocityThreshold = 0.35f;
            SituationalVelocityWindow = 1.5f;
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}\n" +
                $"Velocity threshold: {VelocityThreshold}\n" +
                $"Situational velocity window: {SituationalVelocityWindow}";
        }
    }
}