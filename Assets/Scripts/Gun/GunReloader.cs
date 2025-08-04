using UnityEngine.UI;
using UnityEngine;
using TheSentinel.Skills;
using TheSentinel.Cores;

namespace TheSentinel.Guns
{
    public class GunReloader : MonoBehaviour
    {
        [SerializeField] private Slider _reloadSlider;

        public bool IsReloading { get; private set; }
        public float ReloadTimer { get; private set; }


        public void HandleReload(GunInventory inventory)
        {
            if (inventory.AnyMachineGunActive() && PathChoice.InfiniteAmmo) return;

            UpdateReloadUI(inventory);

            if (IsReloading)
            {
                ReloadTimer -= Time.deltaTime;
                if (ReloadTimer <= 0)
                    FinalizeReload(inventory);
                return;
            }

            if (ShouldStartReload(inventory))
                StartReload(inventory);
        }

        private bool ShouldStartReload(GunInventory inventory)
        {
            return inventory.Bullets > 0 &&
                    inventory.CurrentGunStats.Chamber < inventory.CurrentGunStats.MaxChamber &&
                    (inventory.CurrentGunStats.Chamber <= 0 || Input.GetKeyDown(KeyCode.R));
        }

        private void StartReload(GunInventory inventory)
        {
            IsReloading = true;
            ReloadTimer = inventory.CurrentGunStats.ReloadTime - inventory.CurrentGunStats.TempReloadTime;
        }

        private void UpdateReloadUI(GunInventory inventory)
        {
            _reloadSlider.maxValue = inventory.CurrentGunStats.ReloadTime - inventory.CurrentGunStats.TempReloadTime;
            _reloadSlider.gameObject.SetActive(IsReloading);
            _reloadSlider.value = (inventory.CurrentGunStats.ReloadTime - inventory.CurrentGunStats.TempReloadTime) - ReloadTimer;
        }

        private void FinalizeReload(GunInventory inventory)
        {
            int missingAmmo = inventory.CurrentGunStats.MaxChamber - inventory.CurrentGunStats.Chamber;
            int ammoToAdd = Mathf.Min(missingAmmo, inventory.Bullets);

            inventory.CurrentGunStats.Chamber += ammoToAdd;
            inventory.Bullets -= ammoToAdd;

            AmmoUIManager.Instance.Reload(inventory.CurrentGunStats.Chamber);
            IsReloading = false;
        }


    }
    
}
