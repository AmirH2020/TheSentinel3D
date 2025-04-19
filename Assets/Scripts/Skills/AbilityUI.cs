using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TheSentinel.Skills
{
    public class AbilityUI : MonoBehaviour
    {
        public GameObject UI;
        public TMP_Text cooldownText;
#nullable enable
        public Slider? durationSlider;
#nullable disable
        public TMP_Text abilityName;
        public TMP_Text activateKey;

    }
}
