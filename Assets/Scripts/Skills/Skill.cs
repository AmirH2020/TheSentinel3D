using TheSentinel.Cores;
using UnityEngine.UI;
using TMPro;
namespace TheSentinel.Skills
{
    public abstract class Skill
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected int _level, _maxLevel;
        public int Level => _level;
        public int MaxLevel => _maxLevel;
        public int Price;
        public bool Completed => _level >= _maxLevel;

        public Image image;
        public TMP_Text levelText;

        public bool Locked;
        public bool HaveSkill { get; protected set; }

        public abstract void Initiation();
        public abstract void Update();
        protected virtual void Initiate(int maxLevel)
        {
            _level = 0;
            _maxLevel = maxLevel;
            HaveSkill = false;
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
                image.gameObject.SetActive((inPlayerHpPath && PathChoice.InfinitePlayerHp) || (inInfiniteAmmoPath && PathChoice.InfiniteAmmo));
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
        
    }
}
