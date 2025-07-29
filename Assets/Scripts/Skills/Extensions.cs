using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using System.Collections.Generic;
using TheSentinel.Cores;

namespace TheSentinel.Skills
{
    public static class Extensions
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
        public static void AddT(this List<Upgrade> upgrades,Upgrade item)
        {
            if (item.IsAvailable())
                upgrades.Add(item);
        }
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
