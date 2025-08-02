using TheSentinel.Cores;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel
{
    public class EnemyGunner : Enemy
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Drop _drop;
        [SerializeField] private Transform _gunPoint;

        private int _score, _targetIndex;
        private float _shootTime,_shootTimer;
        private Transform[] _shootTargets = new Transform[2];
        private EnemyBullet _bulletScript;


        Quaternion _shootRotation;
        protected override void Awake()
        {
            base.Awake();
            _bulletScript = _bullet.GetComponent<EnemyBullet>();

            _targetIndex = Random.Range(0,2);
            
            if (PathChoice.InfinitePlayerHp)
            {
                float rand = Random.Range(0, 100);
                if (rand > 40 && rand < 60)
                    _targetIndex = 0;
                else
                    _targetIndex = 1;

            }

            _shootTimer = 2;
            _shootTargets = new Transform[2] { PlayerScript.Instance.transform, TowerScript.Instance.transform };
            _stopDistance = Random.Range(3f, 5f);

            float temp = Random.Range(1f, 3f);
            if (SpawnerScript._enemyWaveManager?.Wave <= 2 && temp > 2.5f)
                temp = 2.5f;

            _moveSpeed = 2.1f / temp;
            _shootTime = 0.6f * temp;
            _bulletScript.SetDamage((int)(1.9f * temp));

            _score = (int)(20 * temp);

            hpManager.Initialize(6 * temp, _hpSlider,_hpText);
        }

        public void Update()
        {
            hpManager.HPUI();
            hpManager.HPLogic(Die);
            if (GameManager.OnPause) return;
            Moving();
            LookAtTarget();

            _shootTimer = Mathf.Max(0 , _shootTimer - Time.deltaTime);
            if (_shootTimer <= 0)
            {
                Shoot();
                _shootTimer = _shootTime;
            }
        }
        private void LookAtTarget()
        {

            Vector3 PlayerPos = _shootTargets[_targetIndex].position;

            Vector3 pos = PlayerPos - transform.position;
            pos.y = 0;
            Quaternion rot = Quaternion.LookRotation(pos);
            var r = rot.eulerAngles;
            r.Set(r.x, r.y - 90, r.z);

            transform.rotation = Quaternion.Euler(r);
            r = rot.eulerAngles;
            r.Set(r.x, r.y, r.z);
            _shootRotation = Quaternion.Euler(r);

        }
        void Shoot()
        {
            Instantiate(_bullet.gameObject, _gunPoint.position, _shootRotation);
        }
        protected override void Die()
        {
            _drop?.RandomDrop(transform.position);
            GameManager.SetScore(GameManager.Score + _score);
            base.Die();
        }
        public void AssignLevelStats(int l)
        {
            _shootTime -= 0.01f * l;
            _bulletScript.ModifyDamage((int)(0.06 * l));
            hpManager.SetMaxHp(hpManager.MaxHP + 2 * l);
            _score += 5 * l;
            hpManager.SetHp(hpManager.MaxHP);
        }
        protected override void ModifyDamage(float damage)
        {
            _bulletScript.ModifyDamage(damage);
        }

    }
}
