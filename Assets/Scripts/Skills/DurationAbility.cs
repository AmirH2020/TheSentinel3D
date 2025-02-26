using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public abstract class DurationAbility : Ability,IDuration,ICooldown
    {
        [SerializeField] protected Slider _durationSlider;
        protected bool _activationCondition = false;
        protected override void Initiate(float _duration, float _cooldown, float _durationUpgrade, float _cooldownUpgrade, int maxLevel)
        {
            base.Initiate(_duration, _cooldown, _durationUpgrade, _cooldownUpgrade, maxLevel);
            _durationSlider.maxValue = _duration;
        }

        protected override void UI()
        {
            base.UI();
            if (isActive) _durationSlider.value = _durationTimer;
            _durationSlider.gameObject.SetActive(isActive);
        }

        public override void Update()
        {
            UI();
            Timers();
            if (_activationCondition) ActivateAbility();
        }

        protected virtual void ActivateAbility()
        {
            isActive = true;
            _durationTimer = _duration;
            _cooldownTimer = _cooldown;
        }
        private void Timers()
        {
            if (_durationTimer <= 0) isActive = false;
            _durationTimer = Mathf.Max(0, _durationTimer - Time.deltaTime);
            _cooldownTimer = Mathf.Max(0, _cooldownTimer - Time.deltaTime);
        }
        public string GetDuration() => "Duration: " + _duration.ToString() + ((_level != 0 && _level != _maxLevel) ? " + " + _durationUpgrade.ToString() : "") + "s";
        public string GetCooldown() => "Cooldown: " + _cooldown.ToString();
    }

}
