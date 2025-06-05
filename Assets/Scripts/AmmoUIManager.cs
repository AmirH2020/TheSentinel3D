using System.Collections;
using System.Collections.Generic;
using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : Singleton<AmmoUIManager>
{
    [SerializeField] private Texture _bullet;
    [SerializeField] private Texture _bulletShadow;
    [SerializeField] private List<RawImage> _bullets = new List<RawImage>();
    [SerializeField] private GameObject _bulletImage;
    private int _bulletIndex = 14;

    public void ModifyBullets(int chamber,int maxChamber)
    {
        foreach (var bullet in _bullets)
            Destroy(bullet.gameObject);
        _bullets.Clear();
        for(int i = 0; i < maxChamber; i++)
        {
            var g = Instantiate(_bulletImage,transform);
            _bullets.Add(g.GetComponent<RawImage>());
            g.GetComponent<RawImage>().enabled = true;
        }
        _bulletIndex = maxChamber - 1; 


        for(int i = 0;i < maxChamber - chamber; i++) 
            AmmoReduce();
    }

    public void AmmoReduce()
    {
        _bullets[_bulletIndex--].texture = _bulletShadow;
    }
    public void Reload(int ammo)
    {
        _bulletIndex = ammo - 1;
        for (int i = 0; i < ammo ;i++) {

            _bullets[i].texture = _bullet;
        }
    }
}
