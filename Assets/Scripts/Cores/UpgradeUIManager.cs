using System;
using System.Collections.Generic;
using System.Linq;
using TheSentinel.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Cores
{


    public class UpgradeUIManager : Singleton<UpgradeUIManager>
    {
        public List<GameObject> upgradeButtons;
        public List<Button> upgradeButton;
        private IEnumerable<Upgrade> tempUpgrades;
        public void SetUpgradeButtons(int count, IEnumerable<Upgrade> upgrades)
        {
            foreach (var b in upgradeButtons)
                b.gameObject.SetActive(false);

            tempUpgrades = upgrades;

            int i = 0;
            foreach(var b in upgrades)
            {
                upgradeButton[i].onClick.RemoveAllListeners();
                upgradeButtons[i].gameObject.SetActive(true);
                upgradeButtons[i].SetDescriptionText(b.title);
                upgradeButton[i].onClick.AddListener(b.SubAction);
                upgradeButtons[i].GetColorType().color = ColorConvertor.Convert(b.color);
                i++;
            }
        }
    }
}