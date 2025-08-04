using UnityEngine;
using TMPro;
namespace TheSentinel.Guns
{
    public class GunOverheat : MonoBehaviour
    {
        [SerializeField] private float _maxOverheat;
        [SerializeField] private float _coolingDownValue;
        [SerializeField] private TMP_Text overheatAlertText;

        public float Overheat { get; private set; }
        public bool IsOverheated { get; private set; }

        public void Update()
        {
            overheatAlertText.text = IsOverheated ? "Overheated" : "";
            if (Overheat > 0 && (IsOverheated || !Input.GetMouseButton(0)))
                Overheat -= Time.deltaTime * _coolingDownValue;
            else if (Overheat <= 0)
                IsOverheated = false;

            if (Overheat >= _maxOverheat)
                IsOverheated = true;
        }
        public void ModifyOverheat(float overheat)
        {
            Overheat += overheat;
        }
        public void ApplyPunish(GunInventory inventory, GunReloader reloader)
        {
            Overheat++;
        }

    }
    
}
