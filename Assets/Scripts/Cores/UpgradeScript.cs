using System.Collections.Generic;
using TheSentinel.Skills;
using TMPro;
using UnityEngine;

namespace TheSentinel.Cores
{
    public class UpgradeScript : Singleton<UpgradeScript>
    {

        public static bool OnUpgrade = false;

        [SerializeField] private TMP_Text _upgrade1Text, _upgrade2Text;

        private List<Upgrade> _secondUpgrades = new List<Upgrade>();
        private List<Upgrade> _firstUpgrades = new List<Upgrade>();
        private int _upgrade1Index, _upgrade2Index;
        private bool _listsInitiated = false;

        private void Update()
        {
            if (PathChoice.ChoiceMade && !_listsInitiated)
                InitiateLists();
            if (TowerScript.Instance.HaveAnyTurret)
                AddTurretUpgrades();
            if (SkillManager.GetSkill<Shotgun>().HaveSkill)
                AddShotgunUpgrades();
        }
        private int CalculateUpgradeIndex(List<Upgrade> list, TMP_Text textAsset)
        {
            var index = list.Count <= 0 ? -1 : Mathf.Max(Random.Range(0, 2), 0);
            textAsset.text = list.Count <= 0 ? "" : list[index].title;
            return index;
        }
        private void AddShotgunUpgrades()
        {
            _firstUpgrades.Add(new Upgrade("- 0.1f Shotgun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.2f, 1); }));
            _firstUpgrades.Add(new Upgrade("+ 2 Shotgun Damage", () => { GunScript.Instance.ModifyDamage(2, 1); }));
        }
        private void AddTurretUpgrades()
        {
            _secondUpgrades.Add(new Upgrade("- 0.2f Turrets Fire Rate", () => { TowerScript.Instance.ModifyFireRate(0.2f); }));
            _secondUpgrades.Add(new Upgrade("+ 2 Turrets Damage", () => { TowerScript.Instance.ModifyDamage(2); }));
        }
        public void Upgrading()
        {
            var ui = UIManager.Instance;
            ui.TogglePanel(ui.UpgradePanel, true);
            OnUpgrade = true;

            CalculateUpgradeIndex(_firstUpgrades,_upgrade1Text);
            CalculateUpgradeIndex(_secondUpgrades,_upgrade2Text);

            if (_upgrade1Index == -1 && _upgrade2Index == -1)
            {
                ui.TogglePanel(ui.UpgradePanel, false);
                OnUpgrade = false;
            }
        }
        private void InitiateLists()
        {
            if (PathChoice.InfinitePlayerHp)
            {

                _firstUpgrades = new List<Upgrade>
                {
                    new Upgrade("+0.05s Main Gun Fire Rate", () => {GunScript.Instance.ModifyFireRate(0.05f,0); }),
                    new Upgrade("+150 Ammo",() => {GunScript.Instance.AddAmmo(150); }),
                    new Upgrade("+45% Move Speed", () => {UpgradeMethodes.GetMoveSpeed(45); }),
                    new Upgrade("+10 Main Gun Capicity", () => {GunScript.Instance.ModifyMaxChamber(10,0); }),
                    new Upgrade("+150 Ammo",() => {GunScript.Instance.AddAmmo(150); }),
                    new Upgrade("+0.1s Main Gun Fire Rate", () => {GunScript.Instance.ModifyFireRate(0.1f,0); }),
                    new Upgrade("+25% Move Speed", () => {UpgradeMethodes.GetMoveSpeed(25); }),
                };
            }
            else if (PathChoice.InfiniteAmmo)
            {
                _firstUpgrades = new List<Upgrade>
                {
                    new Upgrade("+0.05s Main Gun Fire Rate", () => {GunScript.Instance.ModifyFireRate(0.05f,0); }),
                    new Upgrade("+45% Move Speed", () => {UpgradeMethodes.GetMoveSpeed(45); }),
                    new Upgrade("+20% Player Max HP", () => {UpgradeMethodes.SetMaxHp(20,PlayerScript.Instance); }),
                    new Upgrade("+0.1s Main Gun Fire Rate", () => {GunScript.Instance.ModifyFireRate(0.1f,0); }),
                    new Upgrade("+25% Move Speed", () => {UpgradeMethodes.GetMoveSpeed(25); }),
                    new Upgrade("+20% Player Max HP", () => {UpgradeMethodes.SetMaxHp(20,PlayerScript.Instance); }),
                };
            }
            var tower = TowerScript.Instance;
            var towerHpManager = tower.GetHPManager() as TowerHPManager;
            _secondUpgrades = new List<Upgrade>
            {
                new Upgrade("+20% Tower Max HP", () => {UpgradeMethodes.SetMaxHp(20,tower); }),
                new Upgrade("+1/s Tower HP Regen", () => {towerHpManager.ModifyHPRegen(1); }),
                new Upgrade("Get Left Turret for Tower",() => {tower.ActiveTurret(0); }),
                new Upgrade("Get Right Turret for Tower", () => {tower.ActiveTurret(0); }),
            };
            _listsInitiated = true;
        }
        public void Upgrade(int index)
        {
            var upgradingIndex = index == 1 ? _upgrade1Index : _upgrade2Index;
            var notUpgradingIndex = index == 1 ? _upgrade2Index : _upgrade1Index;
            var upgradingList = index == 1 ? _firstUpgrades : _secondUpgrades;
            var notUpgradingList = index == 1 ? _secondUpgrades : _firstUpgrades;
            var ui = UIManager.Instance;
            ui.TogglePanel(ui.UpgradePanel, false);
            OnUpgrade = false;
            if (upgradingIndex == -1)
                return;

            upgradingList[upgradingIndex].action();
            upgradingList.RemoveAt(upgradingIndex);
            if (notUpgradingIndex != -1)
            {
                notUpgradingList.Add(notUpgradingList[notUpgradingIndex]);
                notUpgradingList.RemoveAt(notUpgradingIndex);
            }
        }
        public void FirstUpgrade() => Upgrade(1);
        public void SecondUpgrade() => Upgrade(2);
    }
}