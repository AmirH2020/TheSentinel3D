using System;
using TMPro;
using TheSentinel.Cores;
using UnityEngine;
namespace TheSentinel.Skills
{
    public class TowerRage : Skill, IDetails
    {
        private string _details;
        private bool _active;

        [SerializeField] private float _fireRate, _damage, _moveSpeed;
        [SerializeField] private float _fireRateUpgrade, _damageUpgrade, _moveSpeedUpgrade;

        public override void Initiation()
        {
            InitiateDescription("Tower Rage", "if the tower has less than 30% hp the player goes into a rage mode which gives him more fire rate, damage and move speed ");

            _details = "Damage : +" + _damage+ "\nFirerate: -" + _fireRate + "s\n_Move speed : +" + _moveSpeed;
            
            _maxLevel = 5;
            _active = false;
        }

        public override void Update()
        {
            PathChoiceSkill(true,true);

            var notUpgradable = _level == 0 || _level == _maxLevel;
            var damageUpgrade = notUpgradable ? "" : " +" + _damageUpgrade.ToString() ;
            var fireRateUpgrade = notUpgradable ? "" : " s -" + _fireRateUpgrade.ToString();
            var moveSpeedUpgrade = notUpgradable ? "" : " +" + _moveSpeedUpgrade.ToString();
            Price = notUpgradable ? 2 : 1;
            _details = "Damage : + " + _damage + damageUpgrade + "\nFirerate: - " + _fireRate + fireRateUpgrade + " s\nMove Speed : +" + _moveSpeed + moveSpeedUpgrade;

            if (!HaveSkill) return;

            PlayerScript.Instance.ToggleRageMode(_active);
            bool tempActive = _active;
            _active = TowerScript.Instance.GetHPManager().isLessThanThreshold(30);
            if(tempActive != _active)
                ApplyModifiers(_active ? _damage : 0, _active ? _fireRate : 0, _active ? _moveSpeed : 0);
            
        }

        private void ApplyModifiers(float damage,float fireRate,float moveSpeed)
        {
            GunScript.Instance.ModifyDamageTemporarily(damage, GunScript.CurrentGunIndex);
            GunScript.Instance.ModifyFireRateTemporarily(fireRate, GunScript.CurrentGunIndex);
            PlayerScript.Instance.ModifyMoveSpeedTemporarily(moveSpeed, GunScript.CurrentGunIndex);
        }

        public override void GetSkill() => HaveSkill = true;

        public override void UpgradeSkill()
        {
            _fireRate += _fireRateUpgrade;
            _damage += _damageUpgrade;
            _moveSpeed += _moveSpeedUpgrade;
            ApplyModifiers(_damage, _fireRate, _moveSpeed);
        }
        public string GetDetails() => _details;


    }
}
