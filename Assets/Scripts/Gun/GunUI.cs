using TMPro;
using UnityEngine;
using TheSentinel.Cores;

namespace TheSentinel.Guns
{
    public class GunUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ammoText;

        public void Initialize(GunStats initialStats)
        {
            AmmoUIManager.Instance.ModifyBullets(initialStats.Chamber, initialStats.MaxChamber);
        }

        public void UpdateAmmoUI(int bullets,float overheat)
        {
            _ammoText.gameObject.SetActive(PathChoice.ChoiceMade && PathChoice.InfinitePlayerHp);
            _ammoText.text = $"AMMO : {bullets}";

            if (!PathChoice.ChoiceMade) return;

            AmmoUIManager.Instance.DefineAmmoUI(PathChoice.InfiniteAmmo);
            if (PathChoice.InfiniteAmmo)
                AmmoUIManager.Instance.ModifySlider(overheat);
        }
    }
    
}
