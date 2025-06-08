using System.Collections;
using System.Collections.Generic;
using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : Singleton<AmmoUIManager>
{
    [SerializeField] private Texture _bullet,_bulletShadow;
    [SerializeField] private List<RawImage> _bullets = new List<RawImage>();
    [SerializeField] private GameObject _bulletImage;
    [SerializeField] private GameObject _overheatObjectUI,_ammoObjectUI;
    [SerializeField] private Slider _overheatSlider;
    private int _bulletIndex = 14;

    public void DefineAmmoUI(bool overheat)
    {
        _overheatObjectUI.SetActive(overheat);
        _ammoObjectUI.SetActive(!overheat);
    }

    public void ModifyBullets(int chamber,int maxChamber)
    {
        if (!PathChoice.ChoiceMade)
            return;
        if (PathChoice.InfiniteAmmo)
            return;

        foreach (var bullet in _bullets)
            Destroy(bullet.gameObject);
        _bullets.Clear();
        for(int i = 0; i < maxChamber; i++)
        {
            var g = Instantiate(_bulletImage, _ammoObjectUI.transform);
            _bullets.Add(g.GetComponent<RawImage>());
            g.GetComponent<RawImage>().enabled = true;
        }
        _bulletIndex = maxChamber - 1; 


        for(int i = 0;i < maxChamber - chamber; i++) 
            AmmoReduce();
    }
    public void AmmoReduce()
    {
        if (PathChoice.InfiniteAmmo && PathChoice.ChoiceMade)
            return;
        _bullets[_bulletIndex--].texture = _bulletShadow;
    }
    public void Reload(int ammo)
    {
        if (PathChoice.InfiniteAmmo && PathChoice.ChoiceMade)
            return;
        _bulletIndex = ammo - 1;
        for (int i = 0; i < ammo ;i++) {

            _bullets[i].texture = _bullet;
        }
    }
    public void ModifySlider(float value) => _overheatSlider.value = value;

}
