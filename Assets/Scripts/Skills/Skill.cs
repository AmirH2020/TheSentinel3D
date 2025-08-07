using TheSentinel.Cores;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
namespace TheSentinel.Skills
{
    public abstract class Skill
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string RequiredSkillsString  { get; protected set; }

        protected int _level, _maxLevel;
        public int Price { get; protected set; }
        public bool Completed => _level >= _maxLevel;
        public Button button { get; private set; }
        public bool Locked {  get; protected set; }
        public bool Available {  get; protected set; }
        public bool HaveSkill { get; protected set; }
        public abstract void Initiation();
        public abstract void Update();

        public List<Skill> RequiredSkills = new List<Skill>();

        protected virtual void Initiate(int maxLevel)
        {
            _level = 0;
            _maxLevel = maxLevel;
            HaveSkill = false;
            Price = 1; 
        }
        protected void InitiateDescription(string name, string description)
        {
            Name = name;
            Description = description;
        }
        protected void PathChoiceSkill(bool inPlayerHpPath,bool inInfiniteAmmoPath)
        {
            if (PathChoice.ChoiceMade)
            {
                Available = (inPlayerHpPath && PathChoice.InfinitePlayerHp) || (inInfiniteAmmoPath && PathChoice.InfiniteAmmo);
                button?.gameObject.SetActive(Available);
            }
        }
        public virtual void GetSkill() { }
        public virtual void UpgradeSkill() { }
        public virtual void Upgrade()
        {
            if (GameManager.SkillPoint < Price)
                return;
            LevelUp();
            GameManager.ModifySkillPoint(-Price);
        }
        protected void LevelUp()
        {
            if (_level == 0)
                GetSkill();
            else
                UpgradeSkill();
            _level++;
        }

        public void SetButton(Button button)
        {
            this.button = button;
        }
        
    }
}
