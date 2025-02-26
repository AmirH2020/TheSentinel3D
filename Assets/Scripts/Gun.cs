using UnityEngine;

namespace TheSentinel
{
    [CreateAssetMenu(fileName = "gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public GameObject Bullet;

        public int MaxChamber = 7, Chamber, Damage, Spread;

        public float FireRate = 0.6f, ReloadTime = 2;

        public Color color;

    }
}
