using System.Numerics;
using YARG.Core.IO;

namespace YARG.Core.Song
{
    internal static class Midi_EliteKeys_Preparser
    {
        private const int ELITE_KEYS_MIN = 21;  // A0
        private const int ELITE_KEYS_MAX = 108; // C8

        public static unsafe bool Parse(YARGMidiTrack track)
        {
            BigInteger statusBitMask = 0;
            var note = default(MidiNote);
            var stats = default(YARGMidiTrack.Stats);
            while (track.ParseEvent(ref stats))
            {
                if (stats.Type is MidiEventType.Note_On or MidiEventType.Note_Off)
                {
                    track.ExtractMidiNote(ref note);
                    if (ELITE_KEYS_MIN <= note.value && note.value <= ELITE_KEYS_MAX)
                    {
                        BigInteger statusMask = 1 << (note.value - ELITE_KEYS_MIN);
                        if (stats.Type == MidiEventType.Note_On && note.velocity > 0)
                        {
                            statusBitMask |= statusMask;
                        }
                        else if ((statusBitMask & statusMask) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
