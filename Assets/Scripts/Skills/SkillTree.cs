using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;
namespace TheSentinel.Skills
{
    public class SkillTree : MonoBehaviour
    {
        public Skill _currentSkill { get; private set; }
        private static Dictionary<int, Skill> _skills = new Dictionary<int, Skill>();

        [SerializeField] protected Button _upgradeButton;
        [SerializeField] private Color _canUpgradeColor;
        [SerializeField] private Color _cantUpgradeColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private TMP_Text _name,_description,_duration,_cooldown,_details,_buttonText,_buttonPriceText;

        bool _completed;

        private void Awake()
        {
            _skills.Clear();
            _skills.Add(0, GameManager.Instance.GetComponent<MachineGun>());
            _skills.Add(1, GameManager.Instance.GetComponent<Shield>());
            _skills.Add(2, GameManager.Instance.GetComponent<TowerHealer>());
            _skills.Add(3, GameManager.Instance.GetComponent<HpTransfer>());
            _skills.Add(4, GameManager.Instance.GetComponent<TowerRage>());
            _skills.Add(5, GameManager.Instance.GetComponent<Shotgun>());
            _skills.Add(6, GameManager.Instance.GetComponent<MechanicalShotgun>());
            _skills.Add(7, GameManager.Instance.GetComponent<Overdrive>());
            _skills.Add(8, GameManager.Instance.GetComponent<TowerShield>());
            _currentSkill = _skills[0];
        }
        public void SetCurrentSkill(int index)
        {
            _currentSkill = _skills[index];
            SkillPanelTexts();
            ButtonUI();
        }

        private void SkillPanelTexts()
        {
            _name.text = _currentSkill.Name;
            _description.text = _currentSkill.Description;
            _duration.text = _currentSkill is IDuration ? (_currentSkill as IDuration).GetDuration() : "";
            _cooldown.text = _currentSkill is ICooldown ? (_currentSkill as ICooldown).GetCooldown() : "";
            _details.text = _currentSkill is IDetails ? (_currentSkill as IDetails).GetDetails() : "";
        }
        private void ButtonUI()
        {
            if (_currentSkill.Locked)
            {
                _upgradeButton.image.color = _cantUpgradeColor;
                _buttonText.text = "Locked";
                _buttonPriceText.text = "";
                return;
            }

            if (!_currentSkill.Completed)
            {
                _upgradeButton.image.color = GameManager.SkillPoint < _currentSkill.Price ? _cantUpgradeColor : _canUpgradeColor;
                _buttonPriceText.text = "-" + _currentSkill.Price.ToString();
                _buttonText.text = _currentSkill.Level != 0 ? "Upgrade" : "Get";
            }
            else
            {
                _upgradeButton.image.color = _completedColor;
                _buttonText.text = "Completed";
                _buttonPriceText.text = "";
            }
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
    }
}
