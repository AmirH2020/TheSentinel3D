using System;
using UnityEngine;

namespace TheSentinel.Cores
{
    public class TowerHPManager : HPManager
    {
        private float _hpRegen;

        public override void Initialize(float maxHP)
        {
            base.Initialize(maxHP);
            _hpRegen = 0;
        }
        
        public override void HPLogic(Action? a)
        {
            _hp = Mathf.Min(_hp + _hpRegen * Time.deltaTime, _maxHp);
            _hpSlider.value = _hp;
            if (_hp < 1)
            {
                var b = a == null ? Die : a;
                b();
            }
        }
        public void ModifyHPRegen(float value)
        {
            _hpRegen += value;
        }

        public void Heal(float value)
        {
            _hp = value > _maxHp - _hp ? _maxHp : _hp + value;
        }

    }
}