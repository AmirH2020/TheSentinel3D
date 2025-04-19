using UnityEngine;
using TMPro;
using TheSentinel.Cores;

namespace TheSentinel.Skills
{
    public class TowerHealer : Ability, IDetails,ICooldown
    {
        private int _healAmount, _healUpgradeAmount;
        public override void Initiation()
        {
            Initiate(0, 30, 0, 5, 5);
            _activateKey = KeyCode.H;
            PathChoiceSkill(true, true);
            InitiateDescription("Tower Healer", "Heals the tower!");
            
            _healAmount = 25;
            _healUpgradeAmount = 10;

        }
        public override void Update()
        {
            UI();
            _cooldownTimer = Mathf.Max(0, _cooldownTimer - Time.deltaTime);
            bool activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0;
            if (activationCondition)
            {
                Heal();
                _cooldownTimer = _cooldown;
            }
        }

        private void Heal()
        {
            var towerHpManger = TowerScript.Instance.GetHPManager() as TowerHPManager;
                towerHpManger.Heal(_healAmount);
        }


        public string GetCooldown()
        {
            string cooldownUpgrade = (_level != 0 && _level != _maxLevel) ? " - " + _cooldownUpgrade.ToString() : "";
            return "Cooldown: " + _cooldown.ToString() + cooldownUpgrade + "s";
        }

        public string GetDetails()
        {
            string upgrade = (_level != 0 && _level != _maxLevel) ? " + " + _healUpgradeAmount.ToString() : "";
            return "Heal : " + _healAmount + upgrade;
        }
    }
}
