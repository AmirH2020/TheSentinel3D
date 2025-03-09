using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TheSentinel.Cores
{
    public class PlayerHPManager : HPManager
    {
        private Image _hpFillImage;

        public void Initialize(float value, Slider hpSlider, TMP_Text hpText, Image hpFillImage)
        {
            _hpSlider = hpSlider;
            _hpText = hpText;
            _hpFillImage = hpFillImage;

            _maxHp = value;
            _hp = _maxHp;
            _hpSlider.maxValue = _maxHp;
        }
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