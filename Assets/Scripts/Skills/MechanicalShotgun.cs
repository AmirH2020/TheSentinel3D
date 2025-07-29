using System.Xml.Schema;
using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public interface IFireRateSkill
    {
    }

    public class MechanicalShotgun : DurationAbility, IDuration, ICooldown
    {
        [HideInInspector] public float FireRate;
        bool requiredSkillsAdded = false;
        public override void Initiation()
        {
            Initiate(10, 40, 3, 0, 5);
            InitiateDescription("Mechanical Shotgun",
                "Gives you a high fire Rate Shotgun \n Have Fun");
            _activateKey = KeyCode.F;
            FireRate = 0.2f;

        }

        public override void Update()
        {
            base.Update();
            PathChoiceSkill(inPlayerHpPath: true, inInfiniteAmmoPath: true);

            if (PathChoice.ChoiceMade && !requiredSkillsAdded && SkillManager._skillConditionsAdded)
            {
                var s = (SkillManager.GetSkill<Shotgun>());
                RequiredSkills.Add(s);
                RequiredSkills.Add(SkillManager.GetSkillFromInterface<IFireRateSkill>());


                requiredSkillsAdded = true;
            }
            if (requiredSkillsAdded)
            {
                bool locked = false;
                foreach (var skill in RequiredSkills)
                    if (!skill.HaveSkill)
                        locked = true;
                Locked = locked;
            }

           // Locked = !SkillManager.GetSkill<Shotgun>().HaveSkill || !(SkillManager.GetSkill<MachineGun>().HaveSkill || 
              //  SkillManager.GetSkill<Overdrive>().HaveSkill);



            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0 
                && !SkillManager.GetSkill<MachineGun>().HaveSkill;
            
        }
    }
}
