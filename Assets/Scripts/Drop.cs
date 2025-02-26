using UnityEngine;
using TheSentinel.Cores;

namespace TheSentinel
{
    public class Drop : MonoBehaviour
    {
        public static float DropChance = 40;

        [SerializeField] private GameObject _hpDrop, _ammoDrop;

        public void RandomDrop(Vector3 deathPosition)
        {
            float randomNumber = Random.Range(0, 100), dropChance = (100f - DropChance) / 2;

            if (randomNumber > dropChance && randomNumber < 100 - dropChance)
            {
                bool lowHp = PlayerScript.Instance.GetHPManager().HP < PlayerScript.Instance.GetHPManager().MaxHP * 20f / 100 
                    || TowerScript.Instance.GetHPManager().HP < TowerScript.Instance.GetHPManager().MaxHP * 20f / 100;

                var drop = _hpDrop;

                if (lowHp && PathChoice.InfiniteAmmo)
                    drop = _hpDrop;
                else if (PathChoice.InfinitePlayerHp)
                    drop = _ammoDrop;

                Instantiate(drop, deathPosition, Quaternion.identity);

            }
        }
    }
}
