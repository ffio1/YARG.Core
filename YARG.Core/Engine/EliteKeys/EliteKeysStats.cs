using System.IO;
using YARG.Core.Extensions;
using YARG.Core.IO;
using YARG.Core.Replays;

namespace YARG.Core.Engine.EliteKeys
{
    public class EliteKeysStats : BaseStats
    {
        /// <summary>
        /// Number of overhits which have occurred.
        /// </summary>
        public int Overhits;

        public EliteKeysStats()
        {
        }

        public EliteKeysStats(EliteKeysStats stats) : base(stats)
        {
            Overhits = stats.Overhits;
        }

        public EliteKeysStats(ref FixedArrayStream stream, int version)
            : base(ref stream, version)
        {
            Overhits = stream.Read<int>(Endianness.Little);
        }

        public override void Reset()
        {
            base.Reset();
            Overhits = 0;
        }

        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Overhits);
        }

        public override ReplayStats ConstructReplayStats(string name)
        {
            return new EliteKeysReplayStats(name, this);
        }
    }
}