using System.Collections.Generic;
using UnityEngine;

namespace TheSentinel
{
    public class TurretScript : MonoBehaviour
    {
        [SerializeField] private Transform _gun,_gunPoint;
        [SerializeField] private GameObject _bullet;

        
        private float _shootTimer;
        private float _shootTime;

        private List<Transform> _targets = new List<Transform>();

        private void Start()
        {
            _shootTime = 0.7f;
            _shootTimer = 0.7f;
            _bullet.GetComponent<BulletScript>().SetDamage(2);
        }

        private void Update()
        {

            if (_targets.Count > 0)
            {
                if (_targets[0] == null)
                {
                    _targets.RemoveAt(0);
                }
                else if(_targets.Count > 0)
                {
                    LookAtTarget();
                    if (_shootTimer > 0)
                    {
                        _shootTimer -= Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        _shootTimer = _shootTime;
                    }
                }
            }
        }

        private void LookAtTarget()
        {
            #region Look At The Target
            Vector3 target = _targets[0].transform.position;
            target.x = target.x - transform.position.x;
            target.y = target.y - transform.position.y;
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            _gun.rotation = Quaternion.Lerp(_gun.rotation, Quaternion.Euler(0, 0, angle), 0.5f);
            #endregion
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("enemy"))
            {
                _targets.Add(collision.transform);
            }
        }

        void Shoot()
        {
            Instantiate(_bullet, _gunPoint.position, _gunPoint.rotation);
        }

        public void ModifyFireRate(float value)
        {
            _shootTime -= value;
        }
        public void ModifyDamage(float value)
        {
            _bullet.GetComponent<BulletScript>().SetDamage(_bullet.GetComponent<BulletScript>().Damage + value);
        }
    }
}
