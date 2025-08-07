using TheSentinel.Cores;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace TheSentinel
{
    public enum Target
    {
        Player,
        Tower
    }
    public class EnemyGunner : Enemy
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Drop _drop;
        [SerializeField] private Transform _gunPoint;

        private int _score;
        private Target _targetIndex;

        private Vector3 _target;

        private float _shootTime,_shootTimer;
        private EnemyBullet _bulletScript;


        Vector3 TowerPosition, PlayerPosition;


        Quaternion _shootRotation;
        protected override void Awake()
        {
            base.Awake();
            _bulletScript = _bullet.GetComponent<EnemyBullet>();

            UpdatePositions();

            float rand = Random.Range(0, 100);
            if (PathChoice.InfinitePlayerHp)
            {
                _targetIndex = rand > 40 && rand < 60 ? Target.Player : Target.Tower;
                _target = rand > 40 && rand < 60 ? PlayerPosition : TowerPosition;
            }
            else
            {
                _targetIndex = rand > 50 ? Target.Player : Target.Tower;
                _target = rand > 50 ? PlayerPosition : TowerPosition;
            }




            _shootTimer = 2;
            _stopDistance = Random.Range(3f, 5f);

            float temp = Random.Range(1f, 3f);
            if (SpawnerScript._enemyWaveManager?.Wave <= 2 && temp > 2.5f)
                temp = 2.5f;

            _moveSpeed = 2.1f / temp;
            _shootTime = 0.6f * temp;
            _bulletScript.SetDamage((int)(1.9f * temp));

            _score = (int)(20 * temp);

            hpManager.Initialize(6 * temp, _hpSlider, _hpText);
        }

        private void UpdatePositions()
        {
            TowerPosition = TowerScript.Instance.transform.position;
            TowerPosition.y = transform.position.y;
            PlayerPosition = PlayerScript.Instance.transform.position;
            PlayerPosition.y = transform.position.y;
        }

        public void Update()
        {
            hpManager.HPUI();
            hpManager.HPLogic(Die);
            if (GameManager.OnPause) return;

            Moving();
            UpdatePositions();


            bool shootingCondition = false;
            var distanceToPlayer= Vector3.Distance(transform.position, PlayerPosition);
            bool nearTower = Vector3.Distance(transform.position, TowerPosition) < 8f;

            _shootTimer = Mathf.Max(0 , _shootTimer - Time.deltaTime);



            if(_targetIndex == Target.Player)
            {
                LookAtTarget(distanceToPlayer < 8 ? PlayerPosition : TowerPosition);
                shootingCondition = distanceToPlayer < 8 ? true : false;
            }
            else
            {
                LookAtTarget(TowerPosition);
            }


            if (nearTower) shootingCondition = true;



            if (shootingCondition && _shootTimer <= 0)
            {
                Shoot();
                _shootTimer = _shootTime;
            }
        }
        private void LookAtTarget(Vector3 target)
        {
            Vector3 TargetPos = target;
            Vector3 pos = TargetPos - transform.position;
            pos.y = 0;
            Quaternion rot = Quaternion.LookRotation(pos);
            var r = rot.eulerAngles;
            r.Set(r.x, r.y - 90, r.z);

            transform.rotation = Quaternion.Euler(r);
            r = rot.eulerAngles;
            r.Set(r.x, r.y, r.z);
            _shootRotation = Quaternion.Euler(r);

        }
        void Shoot() => Instantiate(_bullet.gameObject, _gunPoint.position, _shootRotation);
        protected override void Die()
        {
            _drop?.RandomDrop(transform.position);
            GameManager.SetScore(GameManager.Score + _score);
            base.Die();
        }
        public void AssignLevelStats(int l)
        {
            _shootTime -= 0.01f * l;
            _bulletScript.ModifyDamage((int)(0.03 * l));
            hpManager.SetMaxHp(hpManager.MaxHP + 1.5f * l);
            _score += 5 * l;
            hpManager.SetHp(hpManager.MaxHP);
        }
        protected override void ModifyDamage(float damage) => _bulletScript.ModifyDamage(damage);


    }
}
