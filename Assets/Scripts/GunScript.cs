using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using TheSentinel.Skills;


namespace TheSentinel.Cores
{
    public class GunScript : Singleton<GunScript>
    {
        public static int CurrentGunIndex;

        [SerializeField] private float _switchGunTime;
        [SerializeField] private KeyCode _switchGunKey;
        [SerializeField] private List<Gun> guns = new List<Gun>();
        [SerializeField] private List<Gun> allGuns = new List<Gun>();
        [SerializeField] private Gun _shotGun;
        [SerializeField] private Transform _gunPoint;

        [SerializeField] private Slider _reloadSlider;

        private bool _reloading = false;
        private float _reloadTimer, _fireRateTimer, _switchGunTimer;
        private int _bullets;
        private List<GunStats> _gunStats = new List<GunStats>();
        private MachineGun _machineGun;
        private MechanicalShotgun _mechanicalShotgun;

        private void Start()
        {
            _machineGun = SkillManager.GetSkill<MachineGun>();
            _mechanicalShotgun = SkillManager.GetSkill<MechanicalShotgun>();
            _bullets = 150;
            InitiatingGunStats();
            _reloadSlider.maxValue = _gunStats[CurrentGunIndex].ReloadTime - _gunStats[CurrentGunIndex].TempReloadTime;
            _reloadSlider.gameObject.SetActive(false);
            AssignGun(0);
        }
        private void Update()
        {
            Reloading();
            CheckAndSwitchGuns();

            _fireRateTimer = Mathf.Max(0,_fireRateTimer - Time.deltaTime);
            _switchGunTimer = Mathf.Max(0,_switchGunTimer - Time.deltaTime);

            if (Input.GetKeyDown(_switchGunKey)) SwitchGun();

            if (Input.GetMouseButton(0)) Shoot();
        }
        private void CheckAndSwitchGuns()
        {
            bool[] MachineGunsActive = new bool[2] { _machineGun?.isActive ?? false, _mechanicalShotgun?.isActive ?? false };

            if (MachineGunsActive[0] || MachineGunsActive[1]) _reloading = false;

            
            if (MachineGunsActive[0] && CurrentGunIndex == 1)
                AssignGun(0);
            else if (MachineGunsActive[1] && CurrentGunIndex == 0)
                AssignGun(1);

        }
        private void SwitchGun()
        {
            if (SkillManager.GetSkill<MachineGun>().isActive || SkillManager.GetSkill<MechanicalShotgun>().isActive)
                return;

            if (!_reloading && _switchGunTimer <= 0)
            {
                int temp = CurrentGunIndex + 1;
                if (temp >= guns.Count)
                {
                    temp = 0;
                }
                AssignGun(temp);
                _switchGunTimer = _switchGunTime;
            }
        }
        private void Reloading()
        {
            _reloadSlider.gameObject.SetActive(_reloading);
            _reloadSlider.value = (_gunStats[CurrentGunIndex].ReloadTime - _gunStats[CurrentGunIndex].TempReloadTime) - _reloadTimer;

            if ((_machineGun?.isActive ?? false )|| (_mechanicalShotgun?.isActive ?? false)) return;
            
            if (_reloading)
            {
                if (_reloadTimer > 0)
                    _reloadTimer -= Time.deltaTime;
                else
                    Reload();
                return;
            }

            if (!(_bullets > 0 || PathChoice.InfiniteAmmo) && _gunStats[CurrentGunIndex].Chamber >= _gunStats[CurrentGunIndex].MaxChamber) return;

            if (_gunStats[CurrentGunIndex].Chamber <= 0 || Input.GetKeyDown(KeyCode.R))
            {
                _reloading = true;
                _reloadTimer = _gunStats[CurrentGunIndex].ReloadTime - _gunStats[CurrentGunIndex].TempReloadTime;
            }
        }
        private void Shoot()
        {
            if (GameManager.OnPause || _fireRateTimer > 0)
                return;
            var noMachineGun = !(_machineGun?.isActive ?? false) && !(_mechanicalShotgun?.isActive ?? false);
            if (noMachineGun && _gunStats[CurrentGunIndex].Chamber <= 0 || _reloading)
                return;

            _fireRateTimer = noMachineGun ? _gunStats[CurrentGunIndex].FireRate - _gunStats[CurrentGunIndex].TempFireRate:
                _machineGun.isActive ? _machineGun.FireRate : _mechanicalShotgun.FireRate;
            
            Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles);
            for (int i = 0; i < guns[CurrentGunIndex].Spread; i++)
            {
                float offset = Mathf.Pow(-1, i);
                int counter = guns[CurrentGunIndex].Spread % 2 != 0 ? i : i + 1;
                rotation = Quaternion.Euler(rotation.eulerAngles + new Vector3(0, offset * counter * (15f / guns[CurrentGunIndex].Spread),0 ));
                Instantiate(_gunStats[CurrentGunIndex].BulletObject, _gunPoint.position, transform.rotation);
            }
        }
        private void Reload()
        {
            var MissingAmmo = _gunStats[CurrentGunIndex].MaxChamber - _gunStats[CurrentGunIndex].Chamber;
            _gunStats[CurrentGunIndex].Chamber = _bullets >= MissingAmmo || PathChoice.InfiniteAmmo ? _gunStats[CurrentGunIndex].MaxChamber 
                : _gunStats[CurrentGunIndex].Chamber + _bullets;
            _bullets = _bullets >= MissingAmmo && !PathChoice.InfiniteAmmo ? _bullets - MissingAmmo : 0;
            _reloading = false;
            _reloadSlider.gameObject.SetActive(_reloading);
        }
        private void InitiatingGunStats()
        {
            for (int i = 0; i < allGuns.Count; i++)
            {
                GunStats gun = new GunStats();
                gun.MaxChamber = allGuns[i].MaxChamber;
                gun.Chamber = allGuns[i].MaxChamber;
                gun.Damage = allGuns[i].Damage;
                gun.ReloadTime = allGuns[i].ReloadTime;
                gun.FireRate = allGuns[i].FireRate;
                gun.BulletObject = allGuns[i].Bullet;
                gun.BulletObject.GetComponent<BulletScript>().SetDamage(allGuns[i].Damage);
                gun.Bullet = gun.BulletObject.GetComponent<BulletScript>();
                _gunStats.Add(gun);
            }
        }
        private void AssignGun(int id)
        {
            Gun gun = guns[id];
            _reloadSlider.maxValue = _gunStats[id].ReloadTime;

            CurrentGunIndex = id;
            _fireRateTimer = 0;
        }
        public void ModifyDamageTemporarily(float value, int index) => _gunStats[index].Bullet.SetDamage(_gunStats[index].Damage + value);
        public void ModifyDamage(int value, int index) => _gunStats[index].Damage += value;
        public void ModifyFireRateTemporarily(float value, int index) => _gunStats[index].TempFireRate = value;
        public void ModifyFireRate(float value, int index) => _gunStats[index].FireRate -= value;
        public void ModifyReloadTime(float value, int index) =>_gunStats[index].ReloadTime -= value;
        public void AddAmmo(int value) => _bullets += value;
        public void ModifyMaxChamber(int value, int index) =>_gunStats[index].MaxChamber += value;
        public void GetShotgun() => guns.Add(_shotGun);
    }
}