using UnityEngine;

namespace TheSentinel.Cores
{
    public class PathChoice : MonoBehaviour
    {
        public static bool InfiniteAmmo = false;
        public static bool InfinitePlayerHp = false;
        public static bool ChoiceMade => InfiniteAmmo || InfinitePlayerHp;

        UIManager ui = UIManager.Instance;

        public static void ResetPathChoice()
        {
            InfiniteAmmo = false;
            InfinitePlayerHp = false;
        }
        public void GetInfiniteAmmo()
        {
            InfiniteAmmo = true;
            ui.TogglePanel(ui.PathPanel,false);
        }
        public void GetInfinitePlayerHp()
        {
            InfinitePlayerHp = true;
            ui.TogglePanel(ui.PathPanel, false);

        }
    }
}
