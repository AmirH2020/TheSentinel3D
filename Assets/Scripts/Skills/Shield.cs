using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class Shield : DurationAbility, IDuration, ICooldown
    {
        GameObject _playerShield, _towerShield;
        public override void Initiation()
        {
            Initiate(20, 50, 3, 0, 5);
            InitiateDescription("Shield", "Gives you and the tower a shield");
            _activateKey = KeyCode.Q;
            _playerShield = Object.Instantiate(SkillComponents.Instance.ShieldEffect, PlayerScript.Instance.transform);
            _towerShield = Object.Instantiate(SkillComponents.Instance.TowerShieldEffect, TowerScript.Instance.transform);
        }
        public override void Update()
        {
            base.Update();
            _playerShield.SetActive(isActive);
            _towerShield.SetActive(isActive);
            PathChoiceSkill(false, true);

            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0;
        }
    }

}
