using TheSentinel.Cores;
using UnityEngine;
namespace TheSentinel.Skills
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
