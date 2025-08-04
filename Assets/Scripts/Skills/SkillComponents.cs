using TheSentinel.Guns;
using UnityEngine;
namespace TheSentinel.Cores
{
    public class SkillComponents : Singleton<SkillComponents>
    {
        [SerializeField] private Gun _shotgun;
        [SerializeField] private GameObject _shieldEffect;
        [SerializeField] private GameObject _towerShieldEffect;

        public Gun Shotgun { get { return _shotgun; } }
        public GameObject ShieldEffect { get { return _shieldEffect; } }
        public GameObject TowerShieldEffect { get { return _towerShieldEffect; } }
    }
}
