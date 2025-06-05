using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheSentinel.Cores
{
    public class GameManager : Singleton<GameManager>
    {
        public static int SkillPoint { get; private set; } = 10;
        public static int Score { get; private set; } = 0;
        public static bool OnSkillTree { get; private set; } = false;
        public static bool OnPathPanel { get; private set; } = true;   
        public static bool OnPause { get; private set; } = false;

        private static bool _lost = false;
        
        private bool _manualPause;

        private void Update()
        {
            PauseLogic();
            ResetSceneWhenLost();
            SkillPanelLogic();

            if (PathChoice.ChoiceMade)
                OnPathPanel = false;
        }
        private static void SkillPanelLogic()
        {
            if (Input.GetKeyDown(KeyCode.Space) && (OnPause == OnSkillTree))
                OnSkillTree = !OnSkillTree;
            var ui = UIManager.Instance;
            ui.TogglePanel(ui.SkillPanel, OnSkillTree);
        }
        private void ResetSceneWhenLost()
        {
            if (Input.GetMouseButtonDown(0) && _lost)
            {
                _lost = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        private void PauseLogic()
        {
            var ui = UIManager.Instance;
            ui.TogglePanel(ui.PausePanel, _manualPause);

            Time.timeScale = OnPause ? 0 : 1;
            OnPause = _lost || UpgradeScript.OnUpgrade || OnSkillTree || _manualPause || OnPathPanel;
            TogglePause();
        }
        private void TogglePause()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_manualPause || !OnPause)
                {
                    _manualPause = !_manualPause;
                }
                OnPause = !OnPause;
            }
        }
        public static void Lose()
        {
            _lost = true;
            var ui = UIManager.Instance;
            ui.TogglePanel(ui.LosePanel, true);
            PathChoice.ResetPathChoice();
        }
        public static void SetScore(int score)
        {
            Score = score;
        }
        public static void ModifySkillPoint(int value)
        {
            SkillPoint += value;
        }
    }
}
