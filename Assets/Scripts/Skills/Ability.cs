using UnityEngine;

namespace TheSentinel.Skills
{
    public abstract class Ability : Skill
    {
        protected float _duration, _durationUpgrade, _cooldown, _cooldownUpgrade;
        protected float _durationTimer, _cooldownTimer;
        protected AbilityUI abilityUI;

        public void setAbilityUI(AbilityUI abilityUI)
        {
            this.abilityUI = abilityUI;
        }
        public KeyCode _activateKey { get; protected set; }

        public bool isActive { get; protected set; }
        protected virtual void Initiate(float _duration,float _cooldown,float _durationUpgrade,float _cooldownUpgrade,int maxLevel)
        {
            base.Initiate(maxLevel);
            this._duration = _duration;
            this._cooldown = _cooldown;
            this._durationUpgrade = _durationUpgrade;
            this._cooldownUpgrade = _cooldownUpgrade;
        }
        protected virtual void UI()
        {
            abilityUI.name = Name;
            abilityUI.abilityName.text = Name;
            abilityUI.activateKey.text = _activateKey.ToString();
            abilityUI.cooldownText.text = _cooldownTimer > 0 ? ((int)_cooldownTimer).ToString() : "Ready";
            abilityUI.UI.SetActive(HaveSkill);
        }
        public override void GetSkill() => HaveSkill = true;
        public override void UpgradeSkill()
        {
            _cooldown -= _cooldownUpgrade;
            _duration += _durationUpgrade;
        }
    }
}
