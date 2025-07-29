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

        public static bool _skillConditionsAdded { get; private set; } = false;

        private static List<Skill> _skills = new List<Skill>();
        //private static Dictionary<int, Skill> _skills = new Dictionary<int, Skill>();
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
            SkillPanelTexts();
            int abilityIndex = 0;
            int buttonIndex = 0;

            foreach(Button button in _skillButtons)
                button.gameObject.SetActive(false);
            for(int i = 0; i < _skills.Count; i++)
            {
                _skills.Add(_skills[i]);
                _skills.RemoveAt(i);
            }


            foreach (Skill skill in _skills)
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
            int buttonIndex = 0;
            _completed = _currentSkill?.Completed ?? false;
            _skillPointText.text = GameManager.SkillPoint.ToString();

            foreach (Skill skill in _skills)
            {
                if (buttonIndex < _skillButtons.Count)
                {
                    var originalColor = _skillButtons[buttonIndex].gameObject.GetComponent<RawImage>().color;
                    originalColor = new Color(originalColor.r,originalColor.g,originalColor.b,1);
                    var fadedColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
                    _skillButtons[buttonIndex].gameObject.GetComponent<RawImage>().color = skill.Locked ? fadedColor : originalColor;
                    buttonIndex++;
                }
                skill.Update();
            }
            if(!_skillConditionsAdded)
                _skillConditionsAdded = true;
        }
        public void Initialize()
        {
            var skillTypes = Assembly.GetAssembly(typeof(Skill))
                        .GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Skill)) && !(t.IsAbstract))
                        .ToArray();
            foreach (var skillType in skillTypes)
            {
                Skill skill = Activator.CreateInstance(skillType) as Skill;
                _skills.Add(skill);
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
            if(_currentSkill != null)
                ButtonUI();
        }
        public void SkillPanelTexts()
        {
            _name.text = _currentSkill != null ? _currentSkill.Name : "";
            _description.text = _currentSkill != null ? _currentSkill.Description : "";
            _duration.text = _currentSkill != null ? _currentSkill is IDuration ? (_currentSkill as IDuration).GetDuration() : "" : "";
            _cooldown.text = _currentSkill != null ? _currentSkill is ICooldown ? (_currentSkill as ICooldown).GetCooldown() : "" : "";
            _details.text = _currentSkill != null ? _currentSkill is IDetails ? (_currentSkill as IDetails).GetDetails() : "" : "";
            _currentSkillPrice.text = _currentSkill != null ? ("Cost : " + _currentSkill.Price) : "";
        }
        public void Upgrade()
        {
            if (_currentSkill == null)
                return;
            if (_completed)
                return;
            if (_currentSkill.Locked)
                return;

            _currentSkill.Upgrade();

            SkillPanelTexts();
            ButtonUI();
        }

        public static T? GetSkill<T>() where T : Skill => _skills.OfType<T>().Where(t => t.Available).FirstOrDefault();
        public static Skill? GetSkillFromInterface<T>() => _skills.Where(t => t is T && t.Available).FirstOrDefault();

    }
}
