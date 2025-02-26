using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class MechanicalShotgun : DurationAbility, IDuration, ICooldown
    {
        [HideInInspector] public float FireRate;

        public override void Initiation()
        {
            Initiate(10, 40, 3, 0, 5);
            InitiateDescription("Mechanical Shotgun",
                "Gives you a high fire Rate Shotgun \n Have Fun");
            FireRate = 0.2f;
        }

        public override void Update()
        {
            base.Update();
            PathChoiceSkill(inPlayerHpPath: true, inInfiniteAmmoPath: true);
            
            Locked = !SkillManager.GetSkill<Shotgun>().HaveSkill || !(SkillManager.GetSkill<MachineGun>().HaveSkill || 
                SkillManager.GetSkill<Overdrive>().HaveSkill);



            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0 
                && !SkillManager.GetSkill<MachineGun>().HaveSkill;
            
        }
    }
}
