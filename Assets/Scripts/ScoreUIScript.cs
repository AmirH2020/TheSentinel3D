using TMPro;
using TheSentinel.Cores;
using UnityEngine;
using UnityEngine.UI;



namespace TheSentinel
{
    public class ScoreUIScript : MonoBehaviour
    {
        [SerializeField] private GameObject _newPointText;
        [SerializeField] private TMP_Text _skillPointText;


        private Slider _scoreSlider;
        private bool _newPoint;

        private void Awake()
        {
            _newPoint = false;
            _scoreSlider = GetComponent<Slider>();
            _scoreSlider.maxValue = 120;
        }

        void Update()
        {
            _scoreSlider.value = GameManager.Score;
            _skillPointText.text = "Skill Points : " + GameManager.SkillPoint.ToString();
            _newPointText.gameObject.SetActive(_newPoint);

            if (_scoreSlider.value >= _scoreSlider.maxValue)
            {
                GameManager.ModifySkillPoint(1);
                GameManager.SetScore(0);
                _scoreSlider.value = 0;
                _scoreSlider.maxValue += 40 * SpawnerScript._enemyWaveManager.Wave;
                _newPoint = true;
            }

            if (_newPoint && GameManager.OnSkillTree)
            {
                _newPoint = false;
            }
        }
    }
}
