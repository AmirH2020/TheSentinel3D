using UnityEngine;
using System.Collections.Generic;
using TheSentinel.Skills;

namespace TheSentinel.Guns
{
    public class GunInventory : MonoBehaviour
    {
        [SerializeField] public KeyCode SwitchKey;
        [SerializeField] public float SwitchGunTime;
        [SerializeField] public List<Gun> Guns = new List<Gun>();
        [SerializeField] public List<Gun> AllGuns = new List<Gun>();
        [SerializeField] public Gun ShotGun;

        public int Bullets { get; set; }
        public int CurrentGunIndex { get; private set; }
        public List<GunStats> GunStats { get; } = new List<GunStats>();
        public GunStats CurrentGunStats => GunStats[CurrentGunIndex];

        public float SwitchGunTimer { get; set; }
        public float FireRateTimer { get; set; }

        public void Initialize()
        {
            Bullets = 150;
            foreach (Gun gun in AllGuns)
            {
                BulletScript bullet = gun.Bullet.GetComponent<BulletScript>();
                bullet.SetDamage(gun.Damage);

                GunStats.Add(new GunStats
                {
                    MaxChamber = gun.MaxChamber,
                    Chamber = gun.MaxChamber,
                    Damage = gun.Damage,
                    ReloadTime = gun.ReloadTime,
                    FireRate = gun.FireRate,
                    BulletObject = gun.Bullet,
                    Bullet = bullet
                });
            }
            AssignGun(0);
        }

        public void UpdateTimers()
        {
            FireRateTimer = Mathf.Max(0, FireRateTimer - Time.deltaTime);
            SwitchGunTimer = Mathf.Max(0, SwitchGunTimer - Time.deltaTime);
        }

        public void CheckAndSwitchGuns()
        {
            bool machineGunActive = SkillManager.GetSkill<MachineGun>()?.isActive ?? false;
            bool shotgunActive = SkillManager.GetSkill<MechanicalShotgun>()?.isActive ?? false;

            if (machineGunActive && CurrentGunIndex == 1) AssignGun(0);
            else if (shotgunActive && CurrentGunIndex == 0) AssignGun(1);
        }

        public void SwitchGun(GunReloader reloader)
        {
            if (SwitchGunTimer > 0 || reloader.IsReloading || AnyMachineGunActive()) return;

            int newIndex = (CurrentGunIndex + 1) % Guns.Count;
            AssignGun(newIndex);
            SwitchGunTimer = SwitchGunTime;
        }

        public void AssignGun(int id)
        {
            CurrentGunIndex = id;
            GunController.Instance.transform.localScale = Guns[CurrentGunIndex].size;
            AmmoUIManager.Instance.ModifyBullets(CurrentGunStats.Chamber, CurrentGunStats.MaxChamber);
            FireRateTimer = 0;
        }

        public void AddGun(Gun gun) => Guns.Add(gun);
        public bool AnyMachineGunActive()
        {
            return (SkillManager.GetSkill<MachineGun>()?.isActive ?? false) ||
                    (SkillManager.GetSkill<MechanicalShotgun>()?.isActive ?? false);
        }

    }
    
}
