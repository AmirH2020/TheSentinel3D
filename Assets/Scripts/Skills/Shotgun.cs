using TheSentinel.Cores;
using TheSentinel.Guns;
using UnityEngine;
namespace TheSentinel.Skills
{
    public class Shotgun : Skill,IDetails
    {
        private string _details;

        [SerializeField] private float _fireRateUpgrade, _reloadTimeUpgrade;

        private float _fireRate, _reloadTime;

        public override void Initiation()
        {

            _fireRate = SkillComponents.Instance.Shotgun.FireRate;
            _reloadTime = SkillComponents.Instance.Shotgun.ReloadTime;

            InitiateDescription("Shotgun", "Gives a shotgun to the player that has high damage and high fire spread");
            Initiate(5);
            _details = "Fire Rate : " + _fireRate.ToString() + "\nReload Time: " + _reloadTime.ToString();

            
        }
        public override void Update()
        {
            PathChoiceSkill(true,true);
            var notUpgradable = _level == 0 || _level == _maxLevel;
            Price = notUpgradable ? 2 : 1;
            var fireRateUpgrade = notUpgradable ? "" : " - " + _fireRateUpgrade;
            var reloadTimeUpgrade = notUpgradable ? "" : " - " + _reloadTimeUpgrade;
            _details = "Fire Rate : " + _fireRate.ToString() + fireRateUpgrade + "\nReload Time: " + _reloadTime.ToString() + reloadTimeUpgrade;
        }
        public override void GetSkill()
        {
            HaveSkill = true;
            GunController.Instance.GetShotgun();
        }

        public override void UpgradeSkill()
        {
            _fireRate -= _fireRateUpgrade;
            _reloadTime -= _reloadTimeUpgrade;
            GunController.Instance.ModifyFireRate(_fireRateUpgrade,1);
            GunController.Instance.ModifyReloadTime(_reloadTimeUpgrade,1);
        }
        public string GetDetails() => _details;
    }
}
