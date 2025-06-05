using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;

namespace TheSentinel.Skills
{
    public static class GameObjectExtensions
    {
        public static Button GetButton(this GameObject b)
        {
            return b.GetComponentInChildren<Button>();
        }
        public static void SetDescriptionText(this GameObject b, string text)
        {
            b.GetComponentInChildren<TMP_Text>().text = text;
        }
        public static Image GetColorType(this GameObject b)
        {
            var l = b.GetComponentsInChildren<Image>().Where(t => t.gameObject.name == "Type").Take(1).ToList();
            return l[0];
        }
    }
}
