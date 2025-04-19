using UnityEngine;

namespace TheSentinel.Cores
{
    public class PathChoice : MonoBehaviour
    {
        public static bool InfiniteAmmo = false;
        public static bool InfinitePlayerHp = false;
        public static bool ChoiceMade => InfiniteAmmo || InfinitePlayerHp;



        public static void ResetPathChoice()
        {
            InfiniteAmmo = false;
            InfinitePlayerHp = false;
        }
        public void GetInfiniteAmmo()
        {
            InfiniteAmmo = true;
            UIManager.Instance.TogglePanel(UIManager.Instance.PathPanel,false);
        }
        public void GetInfinitePlayerHp()
        {
            InfinitePlayerHp = true;
            UIManager.Instance.TogglePanel(UIManager.Instance.PathPanel, false);

        }
    }
}
