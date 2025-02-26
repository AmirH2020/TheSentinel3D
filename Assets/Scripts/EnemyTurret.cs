
using UnityEngine;

namespace TheSentinel
{
    public class EnemyTurret : Enemy
    {


        public GameObject bullet;
        public GameObject gun;
        public Transform gunPoint;
        public float FireRate,Damage;
        [HideInInspector] public Transform target;
        [HideInInspector] public float _fireRate;

        [HideInInspector] public bool CanShoot;
        public bool isDestroyed {  get; private set; }

        public void Destroy()
        {
            isDestroyed = true;
        }

        private void Awake()
        {
            ModifyDamage(Damage);
        }

        protected override void ModifyDamage(float damage)
        {
            bullet.GetComponent<BulletScript>().ModifyDamage(damage);
        }
        protected override void Die()
        {

            GameObject g = Instantiate(_dieEffect, transform.position, Quaternion.identity);
            Destroy(g, 2);
            Destroy(gameObject);
        }
    }
}
