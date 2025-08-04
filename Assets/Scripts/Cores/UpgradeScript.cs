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
        public List<Upgrade> AllUpgrades = new List<Upgrade>();
        public static bool OnUpgrade = false;

        private int _upgradeCount = 3;
        private bool _listsInitiated = false;
        private UpgradeColor? _lastChosenColor;
        private List<Upgrade> _gotUpgrades = new List<Upgrade>();

        private void Update()
        {
            if (PathChoice.ChoiceMade && !_listsInitiated)
                InitiateLists();
            checkCondition();
        }
        public void Upgrading()
        {
            List<Upgrade> options = new List<Upgrade>();

            var generalFilterList = AllUpgrades.Except(_gotUpgrades).Where(t => t.unlockCondition == true).ToList();
            var sameColorList = generalFilterList.Where(t => t.color == _lastChosenColor).ToList();
            var notSameColorList = generalFilterList.Where(t => t.color != _lastChosenColor).GroupBy(t => t.color).Select(g => g.First()).ToList();
            var differentColorList = generalFilterList.GroupBy(t => t.color).Select(g => g.First()).ToList();
            
            if (_lastChosenColor != null)
            {
                int chosenColorUpgradeCount = (int)( (float)(_upgradeCount * 90) / 100 );
                int otherUpgradeCount = _upgradeCount - chosenColorUpgradeCount;

                for(int i = 0; i < chosenColorUpgradeCount; i++)
                    GetRandomElement(sameColorList, options);

                for (int i = 0; i < _upgradeCount - options.Count; i++)
                    GetRandomElement(notSameColorList, options);

                var newList = generalFilterList.Except(options).ToList();

                while(options.Count < 3 && newList.Count > 0)
                    GetRandomElement(newList, options);

            }
            else
                for (int i = 0; i < _upgradeCount; i++)
                    GetRandomElement(differentColorList, options);

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
            UpgradePopulator.AddUpgrades();
            foreach (var upgrade in AllUpgrades)
                upgrade.SubAction = () => UpgradeAction(upgrade);
            _listsInitiated = true;
        }
        public void UpgradeAction(Upgrade upgrade)
        {
            _lastChosenColor = upgrade.color;

            var nextLevelUpgrades = AllUpgrades.Where(t => t.level == upgrade.level + 1 && t.index == upgrade.index).ToList();
            foreach(var l in nextLevelUpgrades)
                l.setUnlockCondition(true);


            AllUpgrades.Remove(upgrade);
            _gotUpgrades.Add(upgrade);
            upgrade.action();

            var ui = UIManager.Instance;
            ui.TogglePanel(ui.UpgradePanel, false);
            OnUpgrade = false;
        }
        public void checkCondition()
        {
            foreach (var upgrade in AllUpgrades)
                if (upgrade.index == 9)
                    upgrade.setUnlockCondition(SkillManager.GetSkill<Shotgun>()?.HaveSkill ?? false); 
        }
    }
}