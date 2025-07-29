using TheSentinel.Skills;
namespace TheSentinel.Cores
{
    public class UpgradePopulator
    {
        public static void AddUpgrades()
        {
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+0.05s Main Gun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.05f, 0); }, UpgradeColor.Red, true, level: 1, index: 1,UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+0.1s Main Gun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.1f, 0); }, UpgradeColor.Red, false, level: 2, index: 1, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+150 Ammo", () => { GunScript.Instance.AddAmmo(150); }, UpgradeColor.Red, PathChoice.InfinitePlayerHp, level: 1, index: 3,UpgradePath.Hp));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+10 Main Gun Capacity", () => { GunScript.Instance.ModifyMaxChamber(10, 0); }, UpgradeColor.Red, PathChoice.InfinitePlayerHp, level: 1, index: 1, UpgradePath.Hp));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+25% Move Speed", () => { UpgradeMethodes.GetMoveSpeed(25); }, UpgradeColor.Red, true, level: 1, index: 2, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+25% Move Speed", () => { UpgradeMethodes.GetMoveSpeed(25); }, UpgradeColor.Red, false, level: 2, index: 2, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("Decrease Overheating Rate", () => { GunScript.Instance.ModifyOverheatingRate(0.3f); }, UpgradeColor.Red, PathChoice.InfiniteAmmo, level: 1, index: 4, UpgradePath.Ammo));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("Decrease Overheating Rate", () => { GunScript.Instance.ModifyOverheatingRate(0.2f); }, UpgradeColor.Red, false, level: 2, index: 4, UpgradePath.Ammo));


            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+20% Tower Max HP", () => { UpgradeMethodes.SetMaxHp(20, TowerScript.Instance); }, UpgradeColor.Green, true, level: 1, index: 5, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+1/s Tower HP Regen", () => { TowerScript.Instance.hpManager.ModifyHPRegen(1); }, UpgradeColor.Green, true, level: 1, index: 6, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+20% Player Max HP", () => { UpgradeMethodes.SetMaxHp(20, PlayerScript.Instance); }, UpgradeColor.Green, PathChoice.InfiniteAmmo, level: 1, index: 8, UpgradePath.Ammo));


            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("Get Turret for Tower", () => { TowerScript.Instance.ActiveTurret(0); }, UpgradeColor.Purple, true, level: 1, index: 7, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("Get Turret for Tower", () => { TowerScript.Instance.ActiveTurret(1); }, UpgradeColor.Purple, false, level: 2, index: 7, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("- 0.2f Turrets Fire Rate", () => { TowerScript.Instance.ModifyFireRate(0.2f); }, UpgradeColor.Purple, false, level: 2, index: 7, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+ 2 Turrets Damage", () => { TowerScript.Instance.ModifyDamage(2); }, UpgradeColor.Purple, false, level: 2, index: 7, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("- 0.1f Shotgun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.1f, 1); },
                UpgradeColor.Purple, SkillManager.GetSkill<Shotgun>()?.HaveSkill ?? false, level: 1, index: 9, UpgradePath.Both));
            UpgradeScript.Instance.AllUpgrades.AddT(new Upgrade("+ 2 Shotgun Damage", () => { GunScript.Instance.ModifyDamage(2, 1); },
                UpgradeColor.Purple, false, level: 2, index: 9, UpgradePath.Both));

        }
    }
}