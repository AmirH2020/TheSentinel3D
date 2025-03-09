using TheSentinel.Cores;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace TheSentinel
{
    public abstract class Enemy : MonoBehaviour,IHpManager
    {

        protected float _stopDistance;
        [SerializeField] protected Slider _hpSlider;
        [SerializeField] protected TMP_Text _hpText;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected GameObject _dieEffect;
        [SerializeField] private GameObject _damageEffect;
        protected HPManager hpManager;

        protected virtual void Awake()
        {
            hpManager = new HPManager();
        }
        protected virtual void Die()
        {
            //GameObject g = Instantiate(_dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //Destroy(g,2);
        }
        protected void Moving()
        {
            var pos = TowerScript.Instance.transform.position;
            pos.y = transform.position.y;
            if (Vector3.Distance(transform.position, pos) > _stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * _moveSpeed);
            }
        }

        protected abstract void ModifyDamage(float damage);

        public void DamageEffect(Quaternion q)
        {
            //GameObject g = Instantiate(_damageEffect, transform.position, q);
            //Destroy(g, 2);
        }

        public HPManager GetHPManager() => hpManager;
    }
}
