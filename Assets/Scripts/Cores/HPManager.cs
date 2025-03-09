using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

namespace TheSentinel.Cores
{
    [Serializable]
    public class HPManager
    {
        protected float _hp, _maxHp;
        public float HP => _hp;
        public float MaxHP => _maxHp;

        protected Slider _hpSlider;
        public Slider HpSlider => _hpSlider;
        protected TMP_Text _hpText;

        public virtual void Initialize(float value, Slider hpSlider, TMP_Text hpText)
        {
            _hpSlider = hpSlider;
            _hpText = hpText;

            _maxHp = value;
            _hp = _maxHp;
            _hpSlider.maxValue = _maxHp;
        }
        public virtual void HPLogic(Action? a)
        {
            _hp = Mathf.Min(_hp, _maxHp);
            if (_hp < 1)
            {
                var b = a == null ? Die : a;
                b();
            }
            _hpSlider.value = _hp;
        }
        protected virtual void Die() => GameManager.Lose();
        public virtual void SetMaxHp(float value)
        {
            _maxHp = value;
            _hpSlider.maxValue = _maxHp;
        }
        public virtual void SetHp(float value) => _hp = value;
        public virtual void ModifyHP(float value) => _hp += value;
        public virtual bool NotFull() => _hp < _maxHp;
        public virtual void HPUI() => _hpText.text = ((int)_hp).ToString() + "/" + ((int)_maxHp).ToString();
        public virtual bool isLessThanThreshold(float percent) => _hp <= _maxHp * percent/100;
        public virtual bool isGreaterThanThreshold(float percent) => _hp > _maxHp * percent/100;
    }
}