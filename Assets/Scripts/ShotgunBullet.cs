using UnityEngine;

namespace TheSentinel
{
    public class ShotgunBullet : BulletScript
    {
        protected override void Update()
        {
            base.Update();
            if(_damage > 1)
            _damage -= Time.deltaTime * 6;

        }
    }
}