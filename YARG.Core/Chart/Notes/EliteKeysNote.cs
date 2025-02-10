using System;

namespace YARG.Core.Chart
{
    public class EliteKeysNote : Note<EliteKeysNote>
    {
        private EliteKeysNoteFlags _eliteKeysFlags;
        public EliteKeysNoteFlags EliteKeysFlags;

        public int Key { get; }
        public int DisjointMask { get; }
        public int NoteMask { get; private set; }

        public bool IsGlissando => (EliteKeysFlags & EliteKeysNoteFlags.Glissando) != 0;

        public bool IsSustain => TickLength > 0;

        public EliteKeysNote(int key, EliteKeysNoteFlags eliteKeysFlags, NoteFlags flags,
            double time, double timeLength, uint tick, uint tickLength)
            : base(flags, time, timeLength, tick, tickLength)
        {
            Key = key;

            EliteKeysFlags = _eliteKeysFlags = eliteKeysFlags;

            NoteMask = GetKeyMask(Key);
        }

        public EliteKeysNote(EliteKeysNote other) : base(other)
        {
            Key = other.Key;

            EliteKeysFlags = _eliteKeysFlags = other._eliteKeysFlags;

            NoteMask = GetKeyMask(Key);
            DisjointMask = GetKeyMask(Key);
        }

        public override void AddChildNote(EliteKeysNote note)
        {
            if ((NoteMask & GetKeyMask(note.Key)) != 0) return;

            base.AddChildNote(note);

            NoteMask |= GetKeyMask(note.Key);
        }

        public override void ResetNoteState()
        {
            base.ResetNoteState();
            EliteKeysFlags = _eliteKeysFlags;
        }

        protected override void CopyFlags(EliteKeysNote other)
        {
            _eliteKeysFlags = other._eliteKeysFlags;
            EliteKeysFlags = other.EliteKeysFlags;
        }

        protected override EliteKeysNote CloneNote()
        {
            return new(this);
        }

        private static int GetKeyMask(int key)
        {
            return 1 << key;
        }
    }

    [Flags]
    public enum EliteKeysNoteFlags
    {
        None = 0,

        Glissando = 1 << 0,
    }
}