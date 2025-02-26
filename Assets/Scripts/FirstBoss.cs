using TheSentinel.Cores;
using UnityEngine;

namespace TheSentinel
{
    public class FirstBoss : Enemy
    {
        public EnemyTurret leftTurret,rightTurret;
        public GameObject gun,_bullet;
        public Transform _gunPoint;
        public float targetChangeTime, fireRate, Damage;

        private float[] hpPercentPhases = new float[3] {75,45,20};
        private Animator _anim;
        private float _fireRateTimer;
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            leftTurret.GetHPManager().Initialize(100);
            rightTurret.GetHPManager().Initialize(100);
            ModifyDamage(Damage);
            hpManager.Initialize(500);
        }

        private void Start()
        {
            leftTurret.target = PlayerScript.Instance.transform;
            rightTurret.target = TowerScript.Instance.transform;
        }

        void TurretHPLogic(EnemyTurret enemyTurret)
        {
            if (enemyTurret.GetHPManager().HP < 1 && !enemyTurret.isDestroyed)
            {
                enemyTurret.Destroy();
                hpManager.SetHp(hpManager.HP - (30f / 100) * hpManager.HP);
            }
        }

        public void Update()
        {
            _fireRateTimer = _fireRateTimer > 0 ? _fireRateTimer - Time.deltaTime : fireRate;

            if (_fireRateTimer > 0) Shoot(_bullet, _gunPoint.transform.position, gun.transform.rotation);

            //Turning Logic

            TurretHPLogic(leftTurret);
            TurretHPLogic(rightTurret);
            PhaseSet();

            if (!leftTurret.isDestroyed)
                Shooting(leftTurret);

            if (!rightTurret.isDestroyed)
                Shooting(rightTurret);
        }

        private void PhaseSet()
        {
            for(int i = 0; i < 3; i++)
                if(hpManager.isLessThanThreshold(hpPercentPhases[i]))
                    _anim.SetTrigger(i + 1);
        }

        private void Shooting(EnemyTurret turret)
        {
            if (GameManager.OnPause)
                return;
            //LookAtTarget(turret);

            if (turret._fireRate > 0)
                turret._fireRate -= Time.deltaTime;
            else
            { 
                Shoot(turret.bullet.gameObject,turret.gunPoint.position,turret.gun.transform.rotation);
                turret._fireRate = turret.FireRate;
            }
        }
        void Shoot(GameObject bullet,Vector3 gunPoint,Quaternion rotation) => Instantiate(bullet, gunPoint, rotation);
        private void OnDestroy() => hpManager.HpSlider.gameObject.SetActive(false);
        protected override void ModifyDamage(float damage) => _bullet.GetComponent<BulletScript>().ModifyDamage(damage);
    }
}