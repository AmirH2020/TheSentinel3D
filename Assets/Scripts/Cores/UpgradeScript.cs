using System;
using System.Collections.Generic;
using System.Linq;
using TheSentinel.Skills;
using TMPro;
using UnityEngine;
namespace TheSentinel.Cores
{

    public class UpgradeScript : Singleton<UpgradeScript>
    {

        private int _upgradeCount = 3;
        private UpgradeColor? _lastChosenColor;
        private List<Upgrade> _allUpgrades = new List<Upgrade>();
        private List<Upgrade> _gotUpgrades = new List<Upgrade>();

        public static bool OnUpgrade = false;


        private bool _listsInitiated = false;

        private void Update()
        {
            if (PathChoice.ChoiceMade && !_listsInitiated)
                InitiateLists();

            if( _listsInitiated && Input.GetKeyDown(KeyCode.V)) Upgrading();

            checkCondition();

        }

        public void Upgrading()
        {

            List<Upgrade> options = new List<Upgrade>();



            var generalFilterList = _allUpgrades.Except(_gotUpgrades).Where(t => t.unlockCondition == true).ToList();
            var sameColorList = generalFilterList.Where(t => t.color == _lastChosenColor).ToList();
            var notSameColorList = generalFilterList.Where(t => t.color != _lastChosenColor).GroupBy(t => t.color).Select(g => g.First()).ToList();
            var differentColorList = generalFilterList.GroupBy(t => t.color).Select(g => g.First()).ToList();
            
            if (_lastChosenColor != null)
            {
                int chosenColorUpgradeCount = (int)( (float)(_upgradeCount * 90) / 100 );
                int otherUpgradeCount = _upgradeCount - chosenColorUpgradeCount;

                for(int i = 0; i < chosenColorUpgradeCount; i++)
                {
                    GetRandomElement(sameColorList, options);
                }

                for (int i = 0; i < _upgradeCount - options.Count; i++)
                {
                    GetRandomElement(notSameColorList, options);
                }

                var newList = generalFilterList.Except(options).ToList();

                while(options.Count < 3 && newList.Count > 0)
                {
                    GetRandomElement(newList, options);
                }

            }
            else
            {
                for (int i = 0; i < _upgradeCount; i++)
                    GetRandomElement(differentColorList, options);
            }

            var ui = UIManager.Instance;

            if (options.Count > 0)
            {
                ui.TogglePanel(ui.UpgradePanel, true);
                OnUpgrade = true;
            }

            UpgradeUIManager.Instance.SetUpgradeButtons(_upgradeCount, options);

        }

        public void GetRandomElement(List<Upgrade> list,List<Upgrade> options)
        {
            int rand = UnityEngine.Random.Range(0, list.Count());
            if (list.Count() <= 0)
                return;
            options.Add(list[rand]);
            list.RemoveAt(rand);
        }
        private void InitiateLists()
        {
            _allUpgrades.Add(new Upgrade("+0.05s Main Gun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.05f, 0); },UpgradeColor.Red, true,level:1,index:1));
            _allUpgrades.Add(new Upgrade("+0.1s Main Gun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.1f, 0); },UpgradeColor.Red, false,level:2,index:1));
            _allUpgrades.Add(new Upgrade("+25% Move Speed", () => { UpgradeMethodes.GetMoveSpeed(25); }, UpgradeColor.Red, true,level: 1,index:2));
            _allUpgrades.Add(new Upgrade("+10 Main Gun Capacity", () => { GunScript.Instance.ModifyMaxChamber(10, 0); }, UpgradeColor.Red, PathChoice.InfinitePlayerHp,level: 1, index: 3));
            _allUpgrades.Add(new Upgrade("+150 Ammo", () => { GunScript.Instance.AddAmmo(150); }, UpgradeColor.Blue,PathChoice.InfinitePlayerHp,level: 1, index: 4));
            _allUpgrades.Add(new Upgrade("+20% Tower Max HP", () => { UpgradeMethodes.SetMaxHp(20, TowerScript.Instance); },UpgradeColor.Green,true,level: 1, index: 5));
            _allUpgrades.Add(new Upgrade("+1/s Tower HP Regen", () => { TowerScript.Instance.hpManager.ModifyHPRegen(1); },UpgradeColor.Green,true,level: 1, index: 6));
            _allUpgrades.Add(new Upgrade("Get Turret for Tower", () => { TowerScript.Instance.ActiveTurret(0); },UpgradeColor.Purple,true,level: 1, index: 7));
            _allUpgrades.Add(new Upgrade("Get Turret for Tower", () => { TowerScript.Instance.ActiveTurret(1); }, UpgradeColor.Purple, false, level: 2, index: 7));

            _allUpgrades.Add(new Upgrade("+20% Player Max HP", () => { UpgradeMethodes.SetMaxHp(20, PlayerScript.Instance); },UpgradeColor.Green,true,level:1, index: 8));

            _allUpgrades.Add(new Upgrade("- 0.1f Shotgun Fire Rate", () => { GunScript.Instance.ModifyFireRate(0.1f, 1); },

                UpgradeColor.Purple, SkillManager.GetSkill<Shotgun>()?.HaveSkill ?? false,level : 1,index : 9));
            _allUpgrades.Add(new Upgrade("+ 2 Shotgun Damage", () => { GunScript.Instance.ModifyDamage(2, 1); }, 
                UpgradeColor.Purple,false, level: 2, index: 9));


            _allUpgrades.Add(new Upgrade("- 0.2f Turrets Fire Rate", () => { TowerScript.Instance.ModifyFireRate(0.2f); },UpgradeColor.Purple, false, level : 2,index: 7));
            _allUpgrades.Add(new Upgrade("+ 2 Turrets Damage", () => { TowerScript.Instance.ModifyDamage(2); },UpgradeColor.Purple,false, level : 2 , index : 7));


            foreach (var upgrade in _allUpgrades)
                upgrade.SubAction = () => UpgradeAction(upgrade);
            _listsInitiated = true;
        }

        public void UpgradeAction(Upgrade upgrade)
        {
            _lastChosenColor = upgrade.color;

            var nextLevelUpgrades = _allUpgrades.Where(t => t.level == upgrade.level + 1 && t.index == upgrade.index).ToList();
            foreach(var l in nextLevelUpgrades)
                l.setUnlockCondition(true);


            _allUpgrades.Remove(upgrade);
            _gotUpgrades.Add(upgrade);
            upgrade.action();

            var ui = UIManager.Instance;
            ui.TogglePanel(ui.UpgradePanel, false);
            OnUpgrade = false;
        }


        public void checkCondition()
        {
            foreach (var upgrade in _allUpgrades)
                if (upgrade.index == 9)
                    upgrade.setUnlockCondition(SkillManager.GetSkill<Shotgun>()?.HaveSkill ?? false); 

        }
    }
}