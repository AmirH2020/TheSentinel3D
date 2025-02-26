using System.Collections.Generic;
using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;

namespace TheSentinel.Skills
{
    public class SkillUIManager : MonoBehaviour
    {
        public List<Skill> skillList = new List<Skill>();

        public Color lockedColor, unlockedColor, completedColor;

        private void Update()
        {
            ButtonUI();
        }

        protected void ButtonUI()
        {
            foreach (Skill skill in skillList)
            {


                if (skill.Locked) skill.image.color = lockedColor;
                if (skill.Level > 0)
                {
                    skill.image.color = skill.Level == skill.MaxLevel ? completedColor : unlockedColor;
                    skill.levelText.text = skill.Level == skill.MaxLevel ? "Complete" : "Level: " + skill.Level.ToString();
                }
                else
                {
                    skill.image.color = lockedColor;
                    skill.levelText.text = "";
                }
            }
        }

    }
}
