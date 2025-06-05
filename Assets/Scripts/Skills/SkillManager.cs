using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace TheSentinel.Skills
{
    public class SkillManager : MonoBehaviour
    {
        public static Skill _currentSkill { get; private set; }

        private static Dictionary<int, Skill> _skills = new Dictionary<int, Skill>();
        [SerializeField] private TMP_Text _name, _description, _duration, _cooldown, _details, _upgradeButtonText, _skillPointText, _currentSkillPrice;
        [SerializeField] private List<Button> _skillButtons;
        [SerializeField] private List<AbilityUI> abilityUIs;
        [SerializeField] protected Button _upgradeButton;
        [SerializeField] private Color _canUpgradeColor, _cantUpgradeColor, _completedColor;
        private bool _completed;
        


        private void Awake()
        {
            _skills.Clear();
            Initialize();
            _currentSkill = _skills[0];

            int abilityIndex = 0;
            int buttonIndex = 0;
            foreach(Button button in _skillButtons)
                button.gameObject.SetActive(false);

            foreach (Skill skill in _skills.Values)
            {
                if (skill is Ability && abilityIndex < abilityUIs.Count)
                {
                    (skill as Ability).setAbilityUI(abilityUIs[abilityIndex]);
                    abilityUIs[abilityIndex].durationSlider.gameObject.SetActive(skill is DurationAbility);
                    abilityIndex++;
                }
                skill.Initiation();



                if(buttonIndex < _skillButtons.Count)
                {
                    _skillButtons[buttonIndex].gameObject.SetActive(true);
                    _skillButtons[buttonIndex].onClick.AddListener(() => { SetCurrentSkill(skill); });
                    _skillButtons[buttonIndex].GetText().text = skill.Name;
                    skill.SetButton(_skillButtons[buttonIndex]);
                    buttonIndex++;
                }
            }
        }
        public void Update()
        {
            _completed = _currentSkill.Completed;
            _skillPointText.text = GameManager.SkillPoint.ToString();
            _currentSkillPrice.text = "Cost : " + _currentSkill.Price;

            foreach (Skill skill in _skills.Values)
                skill.Update();

        }
        public void Initialize()
        {
            var skillTypes = Assembly.GetAssembly(typeof(Skill))
                        .GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Skill)) && !(t.IsAbstract))
                        .ToArray();
            int id = 0;
            foreach (var skillType in skillTypes)
            {
                Skill skill = Activator.CreateInstance(skillType) as Skill;
                _skills.Add(id,skill);
                id++;
            }
        }
        private void ButtonUI()
        {
            if (_currentSkill.Locked)
            {
                _upgradeButton.image.color = _cantUpgradeColor;
                _upgradeButtonText.text = "Locked";
                return;
            }
            if (!_currentSkill.Completed)
            {
                _upgradeButton.image.color = GameManager.SkillPoint < _currentSkill.Price ? _cantUpgradeColor : _canUpgradeColor;
                _upgradeButtonText.text = "Upgrade";
            }
            else
            {
                _upgradeButton.image.color = _completedColor;
                _upgradeButtonText.text = "Completed";
            }
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
        public void SetCurrentSkill(Skill skill)
        {
            _currentSkill = skill;
            SkillPanelTexts();
            ButtonUI();

        }
        public void SkillPanelTexts()
        {
            _name.text = _currentSkill.Name;
            _description.text = _currentSkill.Description;
            _duration.text = _currentSkill is IDuration ? (_currentSkill as IDuration).GetDuration() : "";
            _cooldown.text = _currentSkill is ICooldown ? (_currentSkill as ICooldown).GetCooldown() : "";
            _details.text = _currentSkill is IDetails ? (_currentSkill as IDetails).GetDetails() : "";
            _currentSkillPrice.text = "Cost : " + _currentSkill.Price;
        }
        public void Upgrade()
        {
            if (_completed)
                return;
            if (_currentSkill.Locked)
                return;
            _currentSkill.Upgrade();

            SkillPanelTexts();
            ButtonUI();
        }
        public static T GetSkill<T>() where T : Skill => _skills.Values.OfType<T>().FirstOrDefault();

    }
}
