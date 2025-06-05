using UnityEngine;

namespace TheSentinel.Cores
{
    public static class ColorConvertor
    {
        public static Color Convert(UpgradeColor color)
        {
            switch (color)
            {
                case UpgradeColor.Red:
                    return new Color(0.3372549f, 0.1883916f, 0.1607843f);
                case UpgradeColor.Green:
                    return new Color(0.1624918f, 0.3372549f, 0.1762623f);
                case UpgradeColor.Blue:
                    return new Color(0.1607843f, 0.244815f, 0.3372549f);
                default:
                    return new Color(0.3372549f, 0.1607843f, 0.3110662f);
            }
        }
    }

}
