using UnityEngine;
using TheSentinel.Cores;

namespace TheSentinel
{
    public class BulletScript : MonoBehaviour
    {
        [SerializeField] protected float _bulletSpeed,_damage;
        public float Damage => _damage;
        protected Rigidbody _rb;

        protected virtual void Awake() => _rb = GetComponent<Rigidbody>();

        protected virtual void Update()
        {
            _rb.AddForce(transform.right * _bulletSpeed);
            Destroy(gameObject, 3);
        }


        public virtual void SetDamage(float damage) => _damage = damage;
        public virtual void ModifyDamage(float damage) => _damage += damage;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IHpManager>() == null)
                return;

            if (other.CompareTag("enemy"))
            {
                HPManager entity = other.GetComponent<IHpManager>().GetHPManager();
                entity.ModifyHP(-_damage);

                Enemy enemy;
                other.TryGetComponent<Enemy>(out enemy);
                if (entity.HP > 1)
                    enemy.DamageEffect(GunScript.Instance?.transform.rotation ?? Quaternion.identity);

                Destroy(gameObject);
            }

            if (other.CompareTag("tower"))
                Destroy(gameObject);
        }
    }
}
