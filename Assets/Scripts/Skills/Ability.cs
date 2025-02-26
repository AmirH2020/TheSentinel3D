using UnityEngine;
using TMPro;

namespace TheSentinel.Skills
{
    public abstract class Ability : Skill
    {
        protected float _duration, _durationUpgrade, _cooldown, _cooldownUpgrade;
        protected float _durationTimer, _cooldownTimer;

        [SerializeField] protected KeyCode _activateKey;
        [SerializeField] protected GameObject _abilityGameUI;   
        [SerializeField] protected TMP_Text _CDText;
        public bool isActive { get; protected set; }
        protected virtual void Initiate(float _duration,float _cooldown,float _durationUpgrade,float _cooldownUpgrade,int maxLevel)
        {
            this._duration = _duration;
            this._cooldown = _cooldown;
            this._durationUpgrade = _durationUpgrade;
            this._cooldownUpgrade = _cooldownUpgrade;
            _level = 0;
            _maxLevel = maxLevel;
            HaveSkill = false;
        }
        protected virtual void UI()
        {
            _CDText.text = _cooldownTimer > 0 ? ((int)_cooldownTimer).ToString() : "Ready";
            _abilityGameUI.SetActive(HaveSkill);
        }
        public override void GetSkill() => HaveSkill = true;
        public override void UpgradeSkill()
        {
            _cooldown -= _cooldownUpgrade;
            _duration += _durationUpgrade;
        }
    }
}
