using System.Collections.Generic;
using TheSentinel.Cores;
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
                    _targets.RemoveAt(0);
                else if(_targets.Count > 0)
                {
                    LookAtTarget();
                    if (_shootTimer <= 0) Shoot();
                    _shootTimer = _shootTimer > 0 ? _shootTimer - Time.deltaTime : _shootTime;
                }
            }
        }

        private void LookAtTarget()
        {
            if (!GameManager.OnPause)
            {
                _gun.transform.LookAt(_targets[0].transform.position);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("enemy"))
                _targets.Add(collision.transform);
        }

        void Shoot()
        {
            var r = _gun.eulerAngles;
            r.Set(r.x, r.y, r.z); 
            Instantiate(_bullet, _gunPoint.position, Quaternion.Euler(r)); 
        }
        public void ModifyFireRate(float value) => _shootTime -= value;
        public void ModifyDamage(float value) => _bullet.GetComponent<BulletScript>().SetDamage(_bullet.GetComponent<BulletScript>().Damage + value);
    }
}
