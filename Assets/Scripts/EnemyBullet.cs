using TheSentinel.Cores;
using UnityEngine;
using TheSentinel.Skills;
namespace TheSentinel
{
    public class EnemyBullet : BulletScript
    {
        protected override void OnTriggerEnter(Collider collision)
        {

            if (collision.GetComponent<IHpManager>() == null)            
                return;

            var NotTakeDamage = false;
            var shield = SkillManager.GetSkill<Shield>()?.isActive ?? false;

            if (collision.CompareTag("Player"))
                NotTakeDamage = PathChoice.InfinitePlayerHp || shield;

            if(collision.CompareTag("tower") && !collision.GetComponent<Collider>().isTrigger)
                NotTakeDamage = (SkillManager.GetSkill<TowerShield>()?.isActive ?? false ) || shield;

            if(NotTakeDamage)
            {
                Destroy(gameObject);
                return;
            }

            collision.GetComponent<IHpManager>()?.GetHPManager().ModifyHP(-_damage);
            Destroy(gameObject);
        }
    }
}