using UnityEngine;
using TheSentinel.Cores;

namespace TheSentinel.Guns
{
    
    public class GunController : Singleton<GunController>
    {
        private GunInventory _inventory;
        private GunShooter _shooter;
        private GunReloader _reloader;
        private GunOverheat _overheat;
        private GunUI _ui;
        private void Start()
        {
            _inventory = GetComponent<GunInventory>();
            _shooter = GetComponent<GunShooter>();
            _ui = GetComponent<GunUI>();
            _reloader = GetComponent<GunReloader>();
            _overheat = GetComponent<GunOverheat>();
            _inventory.Initialize();
            _ui.Initialize(_inventory.CurrentGunStats);
        }

        private void Update()
        {
            _inventory.UpdateTimers();
            _overheat.Update();
            _ui.UpdateAmmoUI(_inventory.Bullets,_overheat.Overheat);

            _inventory.CheckAndSwitchGuns();
            _shooter.HandleShooting(_inventory, _overheat, _reloader);

            _reloader.HandleReload(_inventory);

            if (Input.GetKeyDown(_inventory.SwitchKey))
                _inventory.SwitchGun(_reloader);
        }

        public void AddAmmo(int value) => _inventory.Bullets += value;
        public void GetShotgun() => _inventory.AddGun(_inventory.ShotGun);
        public void ModifyOverheatingRate(float value) => _shooter.ModifyOverheatingRate(value);


        public void ModifyDamageTemporarily(float value, int index) => _inventory.GunStats[index % _inventory.GunStats.Count].Bullet.SetDamage(_inventory.GunStats[index].Damage + value);
        public void ModifyDamage(int value, int index) => _inventory.GunStats[index & _inventory.GunStats.Count].Damage += value; 
        public void ModifyFireRateTemporarily(float value, int index) => _inventory.GunStats[index % _inventory.GunStats.Count].TempFireRate = value;
        public void ModifyFireRate(float value, int index) =>_inventory.GunStats[index % _inventory.GunStats.Count].FireRate -= value; 
        public void ModifyReloadTime(float value, int index) => _inventory.GunStats[index % _inventory.GunStats.Count].ReloadTime -= value; 
        public void ModifyMaxChamber(int value, int index) => _inventory.GunStats[index % _inventory.GunStats.Count].MaxChamber += value;
    }
    
}
