using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class Shield : DurationAbility, IDuration, ICooldown
    {
        GameObject _playerShield, _towerShield;
        [SerializeField] private GameObject _shieldEffect;

        public override void Initiation()
        {
            Initiate(20, 50, 3, 0, 5);
            InitiateDescription("Shield", "Gives you and the tower a shield");
            _playerShield = Object.Instantiate(_shieldEffect, PlayerScript.Instance.transform);
            _towerShield = Object.Instantiate(_shieldEffect, TowerScript.Instance.transform);
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
