namespace YARG.Core.Input
{
    // !DO NOT MODIFY THE VALUES OR ORDER OF THESE ENUMS!
    // Since they are serialized in replays, they *must* remain the same across changes.

    /// <summary>
    /// The actions available for navigating menus.
    /// </summary>
    public enum MenuAction : byte
    {
        /// <summary>Green action button.</summary>
        Green = 0,
        /// <summary>Red action button.</summary>
        Red = 1,
        /// <summary>Yellow action button.</summary>
        Yellow = 2,
        /// <summary>Blue action button.</summary>
        Blue = 3,
        /// <summary>Orange action button.</summary>
        Orange = 4,

        /// <summary>Up navigation button.</summary>
        Up = 5,
        /// <summary>Down navigation button.</summary>
        Down = 6,
        /// <summary>Left navigation button.</summary>
        Left = 7,
        /// <summary>Right navigation button.</summary>
        Right = 8,

        /// <summary>Start action button.</summary>
        Start = 9,
        /// <summary>Select action button.</summary>
        Select = 10,
    }

    /// <summary>
    /// The actions available when playing guitar modes.
    /// </summary>
    public enum GuitarAction : byte
    {
        /// <summary>Generic first fret button.</summary>
        Fret1 = 0,
        /// <summary>Generic second fret button.</summary>
        Fret2 = 1,
        /// <summary>Generic third fret button.</summary>
        Fret3 = 2,
        /// <summary>Generic fourth fret button.</summary>
        Fret4 = 3,
        /// <summary>Generic fifth fret button.</summary>
        Fret5 = 4,
        /// <summary>Generic sixth fret button.</summary>
        Fret6 = 5,

        /// <summary>The up-strum button.</summary>
        StrumUp = 6,
        /// <summary>The down-strum button.</summary>
        StrumDown = 7,

        /// <summary>The whammy axis.</summary>
        Whammy = 8,
        /// <summary>The Star Power action, reported as a button.</summary>
        StarPower = 9,

        /// <summary>(5-fret) Green fret button.</summary>
        /// <remarks>Alias of <see cref="Fret1"/>.</remarks>
        GreenFret = Fret1,
        /// <summary>(5-fret) Red fret button.</summary>
        /// <remarks>Alias of <see cref="Fret2"/>.</remarks>
        RedFret = Fret2,
        /// <summary>(5-fret) Yellow fret button.</summary>
        /// <remarks>Alias of <see cref="Fret3"/>.</remarks>
        YellowFret = Fret3,
        /// <summary>(5-fret) Blue fret button.</summary>
        /// <remarks>Alias of <see cref="Fret4"/>.</remarks>
        BlueFret = Fret4,
        /// <summary>(5-fret) Orange fret button.</summary>
        /// <remarks>Alias of <see cref="Fret5"/>.</remarks>
        OrangeFret = Fret5,

        /// <summary>(6-fret) Black 1 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret1"/>.</remarks>
        Black1Fret = Fret1,
        /// <summary>(6-fret) Black 2 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret2"/>.</remarks>
        Black2Fret = Fret2,
        /// <summary>(6-fret) Black 3 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret3"/>.</remarks>
        Black3Fret = Fret3,
        /// <summary>(6-fret) White 1 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret4"/>.</remarks>
        White1Fret = Fret4,
        /// <summary>(6-fret) White 2 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret5"/>.</remarks>
        White2Fret = Fret5,
        /// <summary>(6-fret) White 3 fret button.</summary>
        /// <remarks>Alias of <see cref="Fret6"/>.</remarks>
        White3Fret = Fret6,
    }

    /// <summary>
    /// The actions available when playing Pro Guitar modes.
    /// </summary>
    public enum ProGuitarAction : byte
    {
        /// <summary>First string's fret number integer.</summary>
        String1_Fret = 0,
        /// <summary>Second string's fret number integer.</summary>
        String2_Fret = 1,
        /// <summary>Third string's fret number integer.</summary>
        String3_Fret = 2,
        /// <summary>Fourth string's fret number integer.</summary>
        String4_Fret = 3,
        /// <summary>Fifth string's fret number integer.</summary>
        String5_Fret = 4,
        /// <summary>Sixth string's fret number integer.</summary>
        String6_Fret = 5,

        /// <summary>First string's strum state, reported as a button.</summary>
        String1_Strum = 6,
        /// <summary>Second string's strum state, reported as a button.</summary>
        String2_Strum = 7,
        /// <summary>Third string's strum state, reported as a button.</summary>
        String3_Strum = 8,
        /// <summary>Fourth string's strum state, reported as a button.</summary>
        String4_Strum = 9,
        /// <summary>Fifth string's strum state, reported as a button.</summary>
        String5_Strum = 10,
        /// <summary>Sixth string's strum state, reported as a button.</summary>
        String6_Strum = 11,

        /// <summary>The Star Power action, reported as a button.</summary>
        StarPower = 12,
    }

    /// <summary>
    /// The actions available when playing Pro Keys modes.
    /// </summary>
    public enum ProKeysAction : byte
    {
        /// <summary>Key 1's press state, reported as a button.</summary>
        Key1 = 0,
        /// <summary>Key 2's press state, reported as a button.</summary>
        Key2 = 1,
        /// <summary>Key 3's press state, reported as a button.</summary>
        Key3 = 2,
        /// <summary>Key 4's press state, reported as a button.</summary>
        Key4 = 3,
        /// <summary>Key 5's press state, reported as a button.</summary>
        Key5 = 4,
        /// <summary>Key 6's press state, reported as a button.</summary>
        Key6 = 5,
        /// <summary>Key 7's press state, reported as a button.</summary>
        Key7 = 6,
        /// <summary>Key 8's press state, reported as a button.</summary>
        Key8 = 7,
        /// <summary>Key 9's press state, reported as a button.</summary>
        Key9 = 8,
        /// <summary>Key 10's press state, reported as a button.</summary>
        Key10 = 9,
        /// <summary>Key 11's press state, reported as a button.</summary>
        Key11 = 10,
        /// <summary>Key 12's press state, reported as a button.</summary>
        Key12 = 11,
        /// <summary>Key 13's press state, reported as a button.</summary>
        Key13 = 12,
        /// <summary>Key 14's press state, reported as a button.</summary>
        Key14 = 13,
        /// <summary>Key 15's press state, reported as a button.</summary>
        Key15 = 14,
        /// <summary>Key 16's press state, reported as a button.</summary>
        Key16 = 15,
        /// <summary>Key 17's press state, reported as a button.</summary>
        Key17 = 16,
        /// <summary>Key 18's press state, reported as a button.</summary>
        Key18 = 17,
        /// <summary>Key 19's press state, reported as a button.</summary>
        Key19 = 18,
        /// <summary>Key 20's press state, reported as a button.</summary>
        Key20 = 19,
        /// <summary>Key 21's press state, reported as a button.</summary>
        Key21 = 20,
        /// <summary>Key 22's press state, reported as a button.</summary>
        Key22 = 21,
        /// <summary>Key 23's press state, reported as a button.</summary>
        Key23 = 22,
        /// <summary>Key 24's press state, reported as a button.</summary>
        Key24 = 23,
        /// <summary>Key 25's press state, reported as a button.</summary>
        Key25 = 24,

        /// <summary>The Star Power action, reported as a button.</summary>
        StarPower = 25,
        /// <summary>The touch effects bar, reported as an axis.</summary>
        TouchEffects = 26,
    }

    /// <summary>
    /// The actions available when playing Elite Keys modes.
    /// </summary>
    public enum EliteKeysAction : byte
    {
        MIDI_OFFSET = 21,

        /// <summary>A0 press state, reported as a button.</summary>
        A0      = 0,
        /// <summary>ASharp0 press state, reported as a button.</summary>
        ASharp0 = 1,
        /// <summary>B0 press state, reported as a button.</summary>
        B0      = 2,
        /// <summary>C1 press state, reported as a button.</summary>
        C1      = 3,
        /// <summary>CSharp1 press state, reported as a button.</summary>
        CSharp1 = 4,
        /// <summary>D1 press state, reported as a button.</summary>
        D1      = 5,
        /// <summary>DSharp1 press state, reported as a button.</summary>
        DSharp1 = 6,
        /// <summary>E1 press state, reported as a button.</summary>
        E1      = 7,
        /// <summary>F1 press state, reported as a button.</summary>
        F1      = 8,
        /// <summary>FSharp1 press state, reported as a button.</summary>
        FSharp1 = 9,
        /// <summary>G1 press state, reported as a button.</summary>
        G1      = 10,
        /// <summary>GSharp1 press state, reported as a button.</summary>
        GSharp1 = 11,
        /// <summary>A1 press state, reported as a button.</summary>
        A1      = 12,
        /// <summary>ASharp1 press state, reported as a button.</summary>
        ASharp1 = 13,
        /// <summary>B1 press state, reported as a button.</summary>
        B1      = 14,
        /// <summary>C2 press state, reported as a button.</summary>
        C2      = 15,
        /// <summary>CSharp2 press state, reported as a button.</summary>
        CSharp2 = 16,
        /// <summary>D2 press state, reported as a button.</summary>
        D2      = 17,
        /// <summary>DSharp2 press state, reported as a button.</summary>
        DSharp2 = 18,
        /// <summary>E2 press state, reported as a button.</summary>
        E2      = 19,
        /// <summary>F2 press state, reported as a button.</summary>
        F2      = 20,
        /// <summary>FSharp2 press state, reported as a button.</summary>
        FSharp2 = 21,
        /// <summary>G2 press state, reported as a button.</summary>
        G2      = 22,
        /// <summary>GSharp2 press state, reported as a button.</summary>
        GSharp2 = 23,
        /// <summary>A2 press state, reported as a button.</summary>
        A2      = 24,
        /// <summary>ASharp2 press state, reported as a button.</summary>
        ASharp2 = 25,
        /// <summary>B2 press state, reported as a button.</summary>
        B2      = 26,
        /// <summary>C3 press state, reported as a button.</summary>
        C3      = 27,
        /// <summary>CSharp3 press state, reported as a button.</summary>
        CSharp3 = 28,
        /// <summary>D3 press state, reported as a button.</summary>
        D3      = 29,
        /// <summary>DSharp3 press state, reported as a button.</summary>
        DSharp3 = 30,
        /// <summary>E3 press state, reported as a button.</summary>
        E3      = 31,
        /// <summary>F3 press state, reported as a button.</summary>
        F3      = 32,
        /// <summary>FSharp3 press state, reported as a button.</summary>
        FSharp3 = 33,
        /// <summary>G3 press state, reported as a button.</summary>
        G3      = 34,
        /// <summary>GSharp3 press state, reported as a button.</summary>
        GSharp3 = 35,
        /// <summary>A3 press state, reported as a button.</summary>
        A3      = 36,
        /// <summary>ASharp3 press state, reported as a button.</summary>
        ASharp3 = 37,
        /// <summary>B3 press state, reported as a button.</summary>
        B3      = 38,
        /// <summary>C4 press state, reported as a button.</summary>
        C4      = 39,
        /// <summary>CSharp4 press state, reported as a button.</summary>
        CSharp4 = 40,
        /// <summary>D4 press state, reported as a button.</summary>
        D4      = 41,
        /// <summary>DSharp4 press state, reported as a button.</summary>
        DSharp4 = 42,
        /// <summary>E4 press state, reported as a button.</summary>
        E4      = 43,
        /// <summary>F4 press state, reported as a button.</summary>
        F4      = 44,
        /// <summary>FSharp4 press state, reported as a button.</summary>
        FSharp4 = 45,
        /// <summary>G4 press state, reported as a button.</summary>
        G4      = 46,
        /// <summary>GSharp4 press state, reported as a button.</summary>
        GSharp4 = 47,
        /// <summary>A4 press state, reported as a button.</summary>
        A4      = 48,
        /// <summary>ASharp4 press state, reported as a button.</summary>
        ASharp4 = 49,
        /// <summary>B4 press state, reported as a button.</summary>
        B4      = 50,
        /// <summary>C5 press state, reported as a button.</summary>
        C5      = 51,
        /// <summary>CSharp5 press state, reported as a button.</summary>
        CSharp5 = 52,
        /// <summary>D5 press state, reported as a button.</summary>
        D5      = 53,
        /// <summary>DSharp5 press state, reported as a button.</summary>
        DSharp5 = 54,
        /// <summary>E5 press state, reported as a button.</summary>
        E5      = 55,
        /// <summary>F5 press state, reported as a button.</summary>
        F5      = 56,
        /// <summary>FSharp5 press state, reported as a button.</summary>
        FSharp5 = 57,
        /// <summary>G5 press state, reported as a button.</summary>
        G5      = 58,
        /// <summary>GSharp5 press state, reported as a button.</summary>
        GSharp5 = 59,
        /// <summary>A5 press state, reported as a button.</summary>
        A5      = 60,
        /// <summary>ASharp5 press state, reported as a button.</summary>
        ASharp5 = 61,
        /// <summary>B5 press state, reported as a button.</summary>
        B5      = 62,
        /// <summary>C6 press state, reported as a button.</summary>
        C6      = 63,
        /// <summary>CSharp6 press state, reported as a button.</summary>
        CSharp6 = 64,
        /// <summary>D6 press state, reported as a button.</summary>
        D6      = 65,
        /// <summary>DSharp6 press state, reported as a button.</summary>
        DSharp6 = 66,
        /// <summary>E6 press state, reported as a button.</summary>
        E6      = 67,
        /// <summary>F6 press state, reported as a button.</summary>
        F6      = 68,
        /// <summary>FSharp6 press state, reported as a button.</summary>
        FSharp6 = 69,
        /// <summary>G6 press state, reported as a button.</summary>
        G6      = 70,
        /// <summary>GSharp6 press state, reported as a button.</summary>
        GSharp6 = 71,
        /// <summary>A6 press state, reported as a button.</summary>
        A6      = 72,
        /// <summary>ASharp6 press state, reported as a button.</summary>
        ASharp6 = 73,
        /// <summary>B6 press state, reported as a button.</summary>
        B6      = 74,
        /// <summary>C7 press state, reported as a button.</summary>
        C7      = 75,
        /// <summary>CSharp7 press state, reported as a button.</summary>
        CSharp7 = 76,
        /// <summary>D7 press state, reported as a button.</summary>
        D7      = 77,
        /// <summary>DSharp7 press state, reported as a button.</summary>
        DSharp7 = 78,
        /// <summary>E7 press state, reported as a button.</summary>
        E7      = 79,
        /// <summary>F7 press state, reported as a button.</summary>
        F7      = 80,
        /// <summary>FSharp7 press state, reported as a button.</summary>
        FSharp7 = 81,
        /// <summary>G7 press state, reported as a button.</summary>
        G7      = 82,
        /// <summary>GSharp7 press state, reported as a button.</summary>
        GSharp7 = 83,
        /// <summary>A7 press state, reported as a button.</summary>
        A7      = 84,
        /// <summary>ASharp7 press state, reported as a button.</summary>
        ASharp7 = 85,
        /// <summary>B7 press state, reported as a button.</summary>
        B7      = 86,
        /// <summary>C8 press state, reported as a button.</summary>
        C8      = 87,
        /// <summary>The Star Power action, reported as a button.</summary>
        StarPower = 88,
        /// <summary>The touch effects bar, reported as an axis.</summary>
        TouchEffects = 89,
    }

    /// <summary>
    /// The actions available when playing drums modes.
    /// </summary>
    public enum DrumsAction : byte
    {
        /// <summary>Generic first drum hit velocity.</summary>
        Drum1 = 0,
        /// <summary>Generic second drum hit velocity.</summary>
        Drum2 = 1,
        /// <summary>Generic third drum hit velocity.</summary>
        Drum3 = 2,
        /// <summary>Generic fourth drum hit velocity.</summary>
        Drum4 = 3,

        /// <summary>Generic first cymbal hit velocity.</summary>
        Cymbal1 = 4,
        /// <summary>Generic second cymbal hit velocity.</summary>
        Cymbal2 = 5,
        /// <summary>Generic third cymbal hit velocity.</summary>
        Cymbal3 = 6,

        /// <summary>Kick pedal hit velocity.</summary>
        Kick = 7,

        /// <summary>(4-lane and 5-lane) red drum hit velocity.</summary>
        /// <remarks>Alias of <see cref="Drum1"/>.</remarks>
        RedDrum = Drum1,
        /// <summary>(4-lane only) yellow drum hit velocity.</summary>
        /// <remarks>Alias of <see cref="Drum2"/>.</remarks>
        YellowDrum = Drum2,
        /// <summary>(4-lane and 5-lane) blue drum hit velocity.</summary>
        /// <remarks>Alias of <see cref="Drum3"/>.</remarks>
        BlueDrum = Drum3,
        /// <summary>(4-lane and 5-lane) green drum hit velocity.</summary>
        /// <remarks>Alias of <see cref="Drum4"/>.</remarks>
        GreenDrum = Drum4,

        /// <summary>(4-lane and 5-lane) Yellow cymbal hit velocity.</summary>
        /// <remarks>Alias of <see cref="Cymbal1"/>.</remarks>
        YellowCymbal = Cymbal1,
        /// <summary>(5-lane only) Orange cymbal hit velocity.</summary>
        /// <remarks>Alias of <see cref="Cymbal2"/>.</remarks>
        OrangeCymbal = Cymbal2,
        /// <summary>(4-lane only) Blue cymbal hit velocity.</summary>
        /// <remarks>Alias of <see cref="Cymbal2"/>.</remarks>
        BlueCymbal = Cymbal2,
        /// <summary>(4-lane only) Green cymbal hit velocity. Red cymbal under lefty flip.</summary>
        /// <remarks>Alias of <see cref="Cymbal3"/>.</remarks>
        GreenCymbal = Cymbal3,
    }

    /// <summary>
    /// The actions available when playing vocals modes.
    /// </summary>
    public enum VocalsAction : byte
    {
        /// <summary>The current pitch being sung.</summary>
        Pitch = 0,
        /// <summary>Percussion hit action, reported as a button..</summary>
        Hit = 1,
        /// <summary>Star Power activation, reported as a button.</summary>
        StarPower = 2,
    }
}