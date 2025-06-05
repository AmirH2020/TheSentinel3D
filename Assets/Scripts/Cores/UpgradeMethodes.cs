using TheSentinel.Skills;
using UnityEngine.Rendering;

namespace TheSentinel.Cores
{
    public class UpgradeMethodes
    {
        public static void GetMoveSpeed(int percent)
        {
            PlayerScript.Instance.AddMoveSpeed(((float)percent / 100) * PlayerScript.MoveSpeed);
        }

        public static void SetMaxHp(float percent,IHpManager entity)
        {
            var hpManager = entity.GetHPManager();
            float temp = hpManager.HP / hpManager.MaxHP;
            hpManager.SetMaxHp(hpManager.MaxHP + (percent / 100) * hpManager.MaxHP);
            hpManager.SetHp(hpManager.MaxHP * temp);
        }
    }
}