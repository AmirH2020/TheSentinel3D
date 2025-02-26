using UnityEngine;
using TheSentinel.Skills;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;



namespace TheSentinel
{
    public class SkillManager : MonoBehaviour
    {
        private static  List<Skill> _skills = new List<Skill>();

        private void Awake()
        {
            Initialize();
            foreach(var skill in _skills)
                skill.Initiation();
        }

        public void Initialize()
        {
            var skillTypes = Assembly.GetAssembly(typeof(Skill)).GetTypes();

            foreach (var skillType in skillTypes)
            {
                Skill skill = Activator.CreateInstance(skillType) as Skill;
                _skills.Add(skill);
            }

        }

        public static T GetSkill<T>() where T : Skill => (T)_skills.Find(skill => skill == (T)skill);
            
        public void Update()
        {
            foreach (Skill skill in _skills)
                skill.Update();
        }
    }
}