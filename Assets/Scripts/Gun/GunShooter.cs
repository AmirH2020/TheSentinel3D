using UnityEngine;
using TheSentinel.Skills;
using TheSentinel.Cores;

namespace TheSentinel.Guns
{
    public class GunShooter : MonoBehaviour
    {
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private float _overheatingRate = 2.5f;

        public void HandleShooting(GunInventory inventory, GunOverheat overheat, GunReloader reloader)
        {
            if (ShouldSkipShooting(inventory, overheat, reloader)) return;

            if (Input.GetMouseButtonDown(0))
                overheat.ApplyPunish(inventory, reloader);

            if (Input.GetMouseButton(0))
                ProcessShooting(inventory, overheat);
        }

        private bool ShouldSkipShooting(GunInventory inventory, GunOverheat overheat, GunReloader reloader)
        {
            return GameManager.OnPause || overheat.IsOverheated || (!inventory.AnyMachineGunActive() && (inventory.CurrentGunStats.Chamber <= 0 || reloader.IsReloading));
        }

        private void ProcessShooting(GunInventory inventory, GunOverheat overheat)
        {
            if (PathChoice.InfiniteAmmo)
                overheat.ModifyOverheat( Time.deltaTime * _overheatingRate);

            if (inventory.FireRateTimer > 0) return;

            inventory.FireRateTimer = inventory.AnyMachineGunActive()
                ? GetActiveAbilityFireRate()
                : inventory.CurrentGunStats.FireRate - inventory.CurrentGunStats.TempFireRate;

            if (PathChoice.InfinitePlayerHp && !inventory.AnyMachineGunActive())
            {
                inventory.CurrentGunStats.Chamber--;
                AmmoUIManager.Instance.AmmoReduce();
            }

            InstantiateBullets(inventory);
        }

        private void InstantiateBullets(GunInventory inventory)
        {
            Vector3 baseRotation = inventory.transform.rotation.eulerAngles + new Vector3(0, 90, 0);
            bool isSpreadEven = inventory.Guns[inventory.CurrentGunIndex].Spread % 2 == 0;

            for (int i = 0; i < inventory.Guns[inventory.CurrentGunIndex].Spread; i++)
            {
                float offset = Mathf.Pow(-1, i);
                int counter = isSpreadEven ? i + 1 : i;
                float angle = offset * counter * (15f / inventory.Guns[inventory.CurrentGunIndex].Spread);
                Quaternion rotation = Quaternion.Euler(baseRotation.x, baseRotation.y + angle, baseRotation.z);
                Instantiate(inventory.CurrentGunStats.BulletObject, _gunPoint.position, rotation);
            }
        }

        private float GetActiveAbilityFireRate()
        {
            var mg = SkillManager.GetSkill<MachineGun>();
            var ms = SkillManager.GetSkill<MechanicalShotgun>();
            return (mg?.isActive ?? false) ? mg.FireRate : ms.FireRate;
        }

        public void ModifyOverheatingRate(float value) => _overheatingRate -= value;
    }
    
}
