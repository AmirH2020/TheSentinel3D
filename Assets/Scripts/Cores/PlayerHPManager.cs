using UnityEngine.UI;
using UnityEngine;

namespace TheSentinel.Cores
{
    public class PlayerHPManager : HPManager
    {
        private Image _hpFillImage;

        public override void HPUI()
        {
            _hpFillImage.color = PathChoice.InfinitePlayerHp ? Color.yellow : Color.red;
         
            if (PathChoice.InfinitePlayerHp)
                _hpText.text = "INFINITE";   
            else
                base.HPUI();
        }
    }
}