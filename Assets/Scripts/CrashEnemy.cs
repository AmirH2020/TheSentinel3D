using System.Runtime.CompilerServices;
using TheSentinel.Cores;
using TheSentinel.Skills;
using UnityEngine;

namespace TheSentinel
{
    public class CrashEnemy : Enemy
    {
        [SerializeField] private float _damage;
        private float distanceToTower, distanceToPlayer;
        private Vector3 towerPos, playerPos;

        protected override void Awake()
        {
            base.Awake();
            _stopDistance = 0;
            hpManager.Initialize(1, _hpSlider, _hpText);
        }
        public void Update()
        {
            hpManager.HPUI();
            hpManager.HPLogic(Die);
            GetPlayerAndTowerPos();
            TurningTowardsTarget();
            CalculateDistance();
            Moving();

            if (distanceToTower < 0.5f)
            {
                Die();
                var shield = (SkillManager.GetSkill<Shield>()?.isActive ?? false) || (SkillManager.GetSkill<TowerShield>()?.isActive ?? false);
                if (!shield)
                    TowerScript.Instance.GetHPManager().ModifyHP(-_damage);
            }
            if (distanceToPlayer < 0.8f)
            {
                Die();
                var shieldOrInfinitePlayerHP = (SkillManager.GetSkill<Shield>()?.isActive ?? false) || PathChoice.InfinitePlayerHp;

                if (!shieldOrInfinitePlayerHP)
                    PlayerScript.Instance.GetHPManager().ModifyHP(-_damage);
            }
        }
        private void CalculateDistance()
        {
            distanceToTower = Vector3.Distance(transform.position, towerPos);
            distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        }
        private void TurningTowardsTarget()
        {
            Vector3 pos = towerPos - transform.position;
            pos.y = 0;
            Quaternion rot = Quaternion.LookRotation(pos);
            var r = rot.eulerAngles;
            r.Set(r.x, r.y - 90, r.z);
            if (!GameManager.OnPause)
                transform.rotation = rot;
        }
        private void GetPlayerAndTowerPos()
        {
            towerPos = TowerScript.Instance.transform.position;
            towerPos.y = transform.position.y;

            playerPos = PlayerScript.Instance.transform.position;
            playerPos.y = transform.position.y;
        }
        protected override void ModifyDamage(float damage) => _damage += damage;
    }
}
