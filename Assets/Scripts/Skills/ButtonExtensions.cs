using UnityEngine.UI;
using TMPro;

namespace TheSentinel.Skills
{
    public static class ButtonExtensions
    {
        public static TMP_Text GetText(this Button b)
        {
            return b.GetComponentInChildren<TMP_Text>();
        }
        public static void SetText(this Button b, string text)
        {
            b.GetComponentInChildren<TMP_Text>().text = text;
        }

    }
}
