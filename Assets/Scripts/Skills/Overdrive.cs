using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class Overdrive : DurationAbility, IDuration,ICooldown
    {
        private bool innateGiven;
        public override void Initiation()
        {
            Initiate(20, 40, 3, 0, 5);
            InitiateDescription("Overdrive",
                "Increases your fire rate for a duration");
            _activateKey = KeyCode.E;
        }

        public override void Update()
        {
            base.Update();
            PathChoiceSkill(inPlayerHpPath: false, inInfiniteAmmoPath: true);

            GunScript.Instance.ModifyFireRateTemporarily(isActive ? 0.25f : 0, 0);
            GunScript.Instance.ModifyFireRateTemporarily(isActive ? 0.15f : 0, 1);


            if (PathChoice.ChoiceMade && !innateGiven && PathChoice.InfiniteAmmo)
            {
                LevelUp();
                innateGiven = true;
            }
            _activationCondition = Input.GetKeyDown(_activateKey) && HaveSkill && _cooldownTimer <= 0 && !SkillManager.GetSkill<MechanicalShotgun>().isActive;
        }
    }
}
