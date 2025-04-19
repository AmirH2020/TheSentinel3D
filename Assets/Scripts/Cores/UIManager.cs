using UnityEngine;

namespace TheSentinel.Cores
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private GameObject _upgradePanel, _losePanel, _skillPanel, _syringePanel, _pathPanel, _pausePanel;
        public GameObject UpgradePanel { get { return _upgradePanel; } }
        public GameObject LosePanel { get { return _losePanel; } }
        public GameObject SkillPanel { get { return _skillPanel; } }
        public GameObject SyringePanel { get { return _syringePanel; } }
        public GameObject PathPanel { get { return _pathPanel; } }
        public GameObject PausePanel { get { return _pausePanel; } }

        private void Start()
        {
            try
            {
                _losePanel.SetActive(false);
                _skillPanel.SetActive(false);
                _syringePanel.SetActive(false);
                _upgradePanel.SetActive(false);
                _pathPanel.SetActive(true);
                _pausePanel.SetActive(false);
            }
            catch
            {
                Debug.LogError("Insert The Correct UI Please");
            }
        }

        public void TogglePanel(GameObject panel, bool toggle)
        {
            try
            {
                panel?.SetActive(toggle);
            }catch
            {
                Debug.LogError("Insert The Correct UI Please");

            }
        }
    }
}