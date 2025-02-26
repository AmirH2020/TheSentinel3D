using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class MachineGun : DurationAbility, IDuration, ICooldown
    {
        public float FireRate { get; private set; }
        public override void Initiation()
        {
            Initiate(10, 30, 3, 0, 5);
            InitiateDescription("Machine Gun",
                "Gives you an infinite ammo machine gun with high fire rate");

            FireRate = 0.1f;
        }

        public override void Update()
        {
            base.Update();
            PathChoiceSkill(inPlayerHpPath: true, inInfiniteAmmoPath: false);

            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0 && !SkillManager.GetSkill<MechanicalShotgun>().isActive;
        }
    }
}
