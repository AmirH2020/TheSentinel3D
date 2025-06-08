using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using TheSentinel.Skills;


namespace TheSentinel.Cores
{
    public class GunScript : Singleton<GunScript>
    {

        [SerializeField] private float _switchGunTime, _maxOverheat,_coolingDownValue;
        [SerializeField] private KeyCode _switchGunKey;
        [SerializeField] private List<Gun> guns = new List<Gun>() , allGuns = new List<Gun>();
        [SerializeField] private Gun _shotGun;
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private Slider _reloadSlider;
        [SerializeField] private TMP_Text _ammoText;

        private int _currentGunIndex,_bullets;
        private bool _reloading = false,_overheated;
        private float _reloadTimer, _fireRateTimer, _switchGunTimer,_overheat;
        private List<GunStats> _gunStats = new List<GunStats>();
        private Ability _machineGun,_mechanicalShotgun;

        private void Start()
        {
            _machineGun = SkillManager.GetSkill<MachineGun>();
            _mechanicalShotgun = SkillManager.GetSkill<MechanicalShotgun>();
            _bullets = 150;
            InitiatingGunStats();
            _reloadSlider.maxValue = _gunStats[_currentGunIndex].ReloadTime - _gunStats[_currentGunIndex].TempReloadTime;
            _reloadSlider.gameObject.SetActive(false);
            AssignGun(0);
        }
        private void Update()
        {
            Reloading();
            CheckAndSwitchGuns();


            _fireRateTimer = Mathf.Max(0, _fireRateTimer - Time.deltaTime);
            _switchGunTimer = Mathf.Max(0, _switchGunTimer - Time.deltaTime);

            if (_overheat > 0 && !Input.GetMouseButton(0))
                _overheat -= Time.deltaTime * _coolingDownValue;
            else if (_overheat <= 0)
                _overheated = false;
            if (_overheat >= _maxOverheat)
                _overheated = true;

            _ammoText.gameObject.SetActive(PathChoice.ChoiceMade && PathChoice.InfinitePlayerHp);
            _ammoText.text = string.Format($"AMMO : {_bullets}");
            if (PathChoice.ChoiceMade)
            {
                AmmoUIManager.Instance.DefineAmmoUI(PathChoice.InfiniteAmmo);



            }
            if (PathChoice.ChoiceMade && PathChoice.InfiniteAmmo)
                AmmoUIManager.Instance.ModifySlider(_overheat);


            if (Input.GetKeyDown(_switchGunKey)) SwitchGun();
            if (Input.GetMouseButton(0)) Shoot();
        }
        private void CheckAndSwitchGuns()
        {
            bool[] MachineGunsActive = new bool[2] { _machineGun?.isActive ?? false, _mechanicalShotgun?.isActive ?? false };

            if (MachineGunsActive[0] || MachineGunsActive[1]) _reloading = false;


            if (MachineGunsActive[0] && _currentGunIndex == 1)
                AssignGun(0);
            else if (MachineGunsActive[1] && _currentGunIndex == 0)
                AssignGun(1);

        }
        private void SwitchGun()
        {
            if (SkillManager.GetSkill<MachineGun>().isActive || SkillManager.GetSkill<MechanicalShotgun>().isActive)
                return;

            if (!_reloading && _switchGunTimer <= 0)
            {
                int temp = _currentGunIndex + 1;
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
            _reloadSlider.value = (_gunStats[_currentGunIndex].ReloadTime - _gunStats[_currentGunIndex].TempReloadTime) - _reloadTimer;

            if ((_machineGun?.isActive ?? false) || (_mechanicalShotgun?.isActive ?? false)) return;

            if (_reloading)
            {
                if (_reloadTimer > 0)
                    _reloadTimer -= Time.deltaTime;
                else
                    Reload();
                return;
            }

            if ((_bullets <= 0 && !PathChoice.InfiniteAmmo) || _gunStats[_currentGunIndex].Chamber >= _gunStats[_currentGunIndex].MaxChamber) return;

            if (_gunStats[_currentGunIndex].Chamber <= 0 || Input.GetKeyDown(KeyCode.R))
            {
                _reloading = true;
                _reloadTimer = _gunStats[_currentGunIndex].ReloadTime - _gunStats[_currentGunIndex].TempReloadTime;
            }
        }
        private void Shoot()
        {
            if (GameManager.OnPause || _fireRateTimer > 0)
                return;
            if (_overheated)
                return;
            var noMachineGun = !(_machineGun?.isActive ?? false) && !(_mechanicalShotgun?.isActive ?? false);
            if (noMachineGun && _gunStats[_currentGunIndex].Chamber <= 0 || _reloading)
                return;

            _fireRateTimer = noMachineGun ? _gunStats[_currentGunIndex].FireRate - _gunStats[_currentGunIndex].TempFireRate :
                _machineGun.isActive ? ((MachineGun)_machineGun).FireRate : ((MechanicalShotgun)_mechanicalShotgun).FireRate;

            if (PathChoice.InfinitePlayerHp)
            {
                _gunStats[_currentGunIndex].Chamber--;
                AmmoUIManager.Instance.AmmoReduce();
            }
            else
            {
                _overheat++;
            }
            Vector3 r = transform.rotation.eulerAngles;
            r.Set(r.x, r.y + 90, r.z);
            Quaternion rotation = Quaternion.Euler(r);
            bool isSpreadEven = guns[_currentGunIndex].Spread % 2 == 0;
            for (int i = 0; i < guns[_currentGunIndex].Spread; i++)
            {
                float offset = Mathf.Pow(-1, i);
                int counter = isSpreadEven ? i + 1 : i;
                rotation = Quaternion.Euler(rotation.eulerAngles + new Vector3(0,offset * counter * (15f / guns[_currentGunIndex].Spread), 0));
                Instantiate(_gunStats[_currentGunIndex].BulletObject, _gunPoint.position, rotation);
            }
        }
        private void Reload()
        {
            if (PathChoice.InfiniteAmmo)
                return;

            var MissingAmmo = _gunStats[_currentGunIndex].MaxChamber - _gunStats[_currentGunIndex].Chamber;
           
            _gunStats[_currentGunIndex].Chamber = _bullets >= MissingAmmo ? _gunStats[_currentGunIndex].MaxChamber : _gunStats[_currentGunIndex].Chamber + _bullets;
            _bullets = _bullets >= MissingAmmo ? _bullets - MissingAmmo : 0;
            AmmoUIManager.Instance.Reload(_gunStats[_currentGunIndex].Chamber);
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
            transform.localScale = guns[id].size;
            _reloadSlider.maxValue = _gunStats[id].ReloadTime;
            _currentGunIndex = id;
            AmmoUIManager.Instance.ModifyBullets(_gunStats[id].Chamber, _gunStats[id].MaxChamber);
            _fireRateTimer = 0;
        }
        public void ModifyDamageTemporarily(float value, int index) {if (index < _gunStats.Count)_gunStats[index].Bullet.SetDamage(_gunStats[index].Damage + value);}
        public void ModifyDamage(int value, int index) { if (index < _gunStats.Count) _gunStats[index].Damage += value; }
        public void ModifyFireRateTemporarily(float value, int index) { if (index < _gunStats.Count) _gunStats[index].TempFireRate = value; }
        public void ModifyFireRate(float value, int index) { if (index < _gunStats.Count) _gunStats[index].FireRate -= value; }
        public void ModifyReloadTime(float value, int index) { if (index < _gunStats.Count) _gunStats[index].ReloadTime -= value;}
        public void AddAmmo(int value) => _bullets += value;
        public void ModifyMaxChamber(int value, int index)  { if (index < _gunStats.Count) _gunStats[index].MaxChamber += value; }
        public void GetShotgun() => guns.Add(_shotGun);
    }
}