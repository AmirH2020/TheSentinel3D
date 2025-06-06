using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradiantSlider : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private Image _fillImage;
    private Color _currentColor;
    private float _hueValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _hueValue = 120f/360;
        _currentColor = Color.HSVToRGB(_hueValue, 1, 65f/100);
    }

    private void Update()
    {
        _hueValue = 120 - (_slider.value/_slider.maxValue * 120);
        _currentColor = Color.HSVToRGB(_hueValue/360, 1, 65f / 100);
        _fillImage.color = _currentColor;
    }

}
