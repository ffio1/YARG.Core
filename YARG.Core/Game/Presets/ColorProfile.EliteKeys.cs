using System.Drawing;
using System.IO;
using YARG.Core.Extensions;
using YARG.Core.Utility;

namespace YARG.Core.Game
{
    public partial class ColorProfile
    {
        public class EliteKeysColors : IBinarySerializable
        {
            #region Keys

            public Color WhiteKey = Color.White;
            public Color BlackKey = Color.Black;

            public Color RedKey = Color.FromArgb(0xFF, 0x7F, 0x00, 0x00); // #7F0000
            public Color DarkRedKey = Color.FromArgb(0xFF, 0x80, 0x0B, 0x0F); // #800B0F
            public Color YellowKey = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00); // #FFFF00
            public Color DarkYellowKey = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00); // #FFFF00
            public Color BlueKey = Color.FromArgb(0xFF, 0x00, 0x80, 0xFF); // #0080FF
            public Color OrangeKey = Color.FromArgb(0xFF, 0xFF, 0x80, 0x00); // #FF8000
            public Color DarkOrangeKey = Color.FromArgb(0xFF, 0xC4, 0x64, 0x02); // #C46402
            public Color GreenKey = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // #00FF00
            public Color DarkGreenKey = Color.FromArgb(0xFF, 0x02, 0xA1, 0x02); // #02A102
            public Color PurpleKey = Color.FromArgb(0xFF, 0x6B, 0x03, 0xFC); // #6B03FC
            public Color DarkPurpleKey = Color.FromArgb(0xFF, 0x34, 0x01, 0x7A); // #34017A
            public Color PinkKey = Color.FromArgb(0xFF, 0xD5, 0x05, 0xF5); // #D505F5

            /// <summary>
            /// Gets the black key color for a specific group index.
            /// 0 = red, 4 = orange.
            /// </summary>
            public Color GetKeyColor(int noteIndex)
            {
                return noteIndex switch
                {
                    0 => RedKey,
                    1 => DarkRedKey,
                    2 => YellowKey,
                    3 => DarkYellowKey,
                    4 => BlueKey,
                    5 => OrangeKey,
                    6 => DarkOrangeKey,
                    7 => GreenKey,
                    8 => DarkGreenKey,
                    9 => PurpleKey,
                    10 => DarkPurpleKey,
                    11 => PinkKey,
                    _ => default
                };
            }

            #endregion

            #region Notes

            public Color WhiteNote = Color.White;
            public Color BlackNote = Color.Black;

            public Color WhiteNoteStarPower = CircularStarpower;
            public Color BlackNoteStarPower = CircularStarpower;

            #endregion

            #region Serialization

            public EliteKeysColors Copy()
            {
                // Kinda yucky, but it's easier to maintain
                return (EliteKeysColors) MemberwiseClone();
            }

            public void Serialize(BinaryWriter writer)
            {
                writer.Write(WhiteKey);

                writer.Write(RedKey);
                writer.Write(DarkRedKey);
                writer.Write(YellowKey);
                writer.Write(DarkYellowKey);
                writer.Write(BlueKey);
                writer.Write(OrangeKey);
                writer.Write(DarkOrangeKey);
                writer.Write(GreenKey);
                writer.Write(DarkGreenKey);
                writer.Write(PurpleKey);
                writer.Write(DarkPurpleKey);
                writer.Write(PinkKey);

                writer.Write(WhiteNote);
                writer.Write(BlackNote);

                writer.Write(WhiteNoteStarPower);
                writer.Write(BlackNoteStarPower);
            }

            public void Deserialize(BinaryReader reader, int version = 0)
            {
                WhiteKey = reader.ReadColor();

                RedKey = reader.ReadColor();
                DarkRedKey = reader.ReadColor();
                YellowKey = reader.ReadColor();
                DarkYellowKey = reader.ReadColor();
                BlueKey = reader.ReadColor();
                OrangeKey = reader.ReadColor();
                DarkOrangeKey = reader.ReadColor();
                GreenKey = reader.ReadColor();
                DarkGreenKey = reader.ReadColor();
                PurpleKey = reader.ReadColor();
                DarkPurpleKey = reader.ReadColor();
                PinkKey = reader.ReadColor();

                RedKey = reader.ReadColor();
                YellowKey = reader.ReadColor();
                BlueKey = reader.ReadColor();
                GreenKey = reader.ReadColor();
                OrangeKey = reader.ReadColor();

                WhiteNote = reader.ReadColor();
                BlackNote = reader.ReadColor();

                WhiteNoteStarPower = reader.ReadColor();
                BlackNoteStarPower = reader.ReadColor();
            }

            #endregion
        }
    }
}