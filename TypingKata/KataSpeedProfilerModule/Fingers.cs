using System.ComponentModel;
using System.Drawing;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Enum that maps color to a finger.
    /// </summary>
    public enum Fingers {
        [FingerEnumColor(KnownColor.SteelBlue)]
        [Description("Left Index")]
        LeftIndex = 0,
        [FingerEnumColor(KnownColor.LightGreen)]
        [Description("Left Middle")]
        LeftMiddle = 1,
        [FingerEnumColor(KnownColor.IndianRed)]
        [Description("Left Ring")]
        LeftRing = 2,
        [FingerEnumColor(KnownColor.Orange)]
        [Description("Left Pinky")]
        LeftPinky = 3,
        [FingerEnumColor(KnownColor.SteelBlue)]
        [Description("Right Index")]
        RightIndex = 4,
        [FingerEnumColor(KnownColor.LightGreen)]
        [Description("Right Middle")]
        RightMiddle = 5,
        [FingerEnumColor(KnownColor.IndianRed)]
        [Description("Right Ring")]
        RightRing = 6,
        [FingerEnumColor(KnownColor.Orange)]
        [Description("Right Pinky")]
        RightPinky = 7,
        [FingerEnumColor(KnownColor.AliceBlue)]
        [Description("Thumb")]
        Thumb = 8
    }
}