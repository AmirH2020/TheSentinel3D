using TheSentinel.Cores;
using UnityEngine;

namespace TheSentinel.Skills
{
    public class HpTransfer : Skill, IDetails
    {
        protected string _details;

        private float _transferRate;


        public override void Initiation()
        {
            InitiateDescription("HpTransfer", "Transfers HP between you and the tower");
            _details = "Transfer Rate: " + _transferRate.ToString();
            Initiate(5);
            _transferRate = 10;
        }

        public override void Update()
        {
            PathChoiceSkill(false, true);

            var ui = UIManager.Instance;
            ui.TogglePanel(ui.SyringePanel, TowerScript.PlayerInRange && HaveSkill && !GameManager.OnPause);

            if (TowerScript.PlayerInRange && HaveSkill)
            {
                if (Input.GetKey(KeyCode.T))
                    Modify(TowerScript.Instance.GetHPManager(), PlayerScript.Instance.GetHPManager());
                else if (Input.GetKey(KeyCode.Y))
                    Modify(PlayerScript.Instance.GetHPManager(), TowerScript.Instance.GetHPManager());
            }
        }

        void Modify(HPManager IncreasingEntity,HPManager DecreasingEntity)
        {
            if(IncreasingEntity.NotFull() && DecreasingEntity.HP > _transferRate)
            {
                IncreasingEntity.ModifyHP(+ (_transferRate * Time.deltaTime));
                DecreasingEntity.ModifyHP(- (_transferRate * Time.deltaTime));
            }
        }

        public string GetDetails() => _details;
        public override void GetSkill() => HaveSkill = true;
    }
}
