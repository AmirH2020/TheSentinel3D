using TheSentinel.Cores;
using UnityEngine;

namespace TheSentinel.Skills
{

    public class TowerShield : DurationAbility, IDuration,ICooldown
    {
        private GameObject _towerShield;

        [SerializeField] private GameObject _shieldEffect;

        public override void Initiation()
        {
            Initiate(_duration: 12,_cooldown: 45,_durationUpgrade:  3,_cooldownUpgrade: 0,maxLevel: 5);
            InitiateDescription("Tower Shield", "Gives the tower a shield");

            _towerShield = Object.Instantiate(_shieldEffect, TowerScript.Instance.transform);
        }
        public override void Update()
        {
            base.Update();
            PathChoiceSkill(true, false);
            _towerShield.SetActive(isActive);
            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0;
        }
    }

}
