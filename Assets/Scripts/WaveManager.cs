using TheSentinel.Cores;
using UnityEngine;
using System.Collections;
using TMPro;


namespace TheSentinel
{
    public class WaveManager
    {

        public int CrashEnemiesNeedToSpawn {  get; private set; }
        public int EnemiesRemaining {  get; private set; }
        public int Wave {  get; private set; }
        public float WaveTimer { get; private set; }

        private bool _waveTimerStarted,_waiting;
        private int[] _numOfEnemiesInWaves = new int[] { 4, 6, 9, 11 };
        private int _crashEnemiesInWaves = 2;
        private TMP_Text _waveText;
        private MonoBehaviour _mono;
        private float _spawnTime;

        public WaveManager(TMP_Text waveText, MonoBehaviour mono, float spawnTime)
        {
            _waveText = waveText;
            _mono = mono;
            _spawnTime = spawnTime;
            Wave = 1;
            WaveTimer = 0;
            EnemiesRemaining = _numOfEnemiesInWaves[0];
            CrashEnemiesNeedToSpawn = _crashEnemiesInWaves;
        }

        public void UpdateWaveTimer(float deltaTime)
        {
            if (_waveTimerStarted)
            {
                WaveTimer -= deltaTime;
                WaveTimer = Mathf.Max(WaveTimer, 0);
            }

        }
        public void StartWaveTimer()
        {
            WaveTimer = 5;
            _waveTimerStarted = true;
        }

        public void HandleWaveProgress()
        {
            if (EnemiesRemaining > 0)
                return;

            if (!_waveTimerStarted)
                StartWaveTimer();
            else if (WaveTimer <= 0 && !_waiting)
                _mono.StartCoroutine(StartNextWave());
        }
        public IEnumerator StartNextWave()
        {
            _waiting = true;
            yield return new WaitForSeconds(2);

            UpgradeScript obj =(UpgradeScript) Object.FindAnyObjectByType(typeof(UpgradeScript));

            obj?.Upgrading();


            EnemiesRemaining = Wave < 4 ? _numOfEnemiesInWaves[Wave] : 5 * Wave;

            Wave++;

            _mono.StartCoroutine(WaveText());

            CrashEnemiesNeedToSpawn = _crashEnemiesInWaves ++;

            _waiting = false;
        }

        public IEnumerator WaveText()
        {
            _waveText.text = "Wave: " + Wave;
            yield return new WaitUntil(() => !GameManager.OnPause);
            yield return new WaitForSeconds(_spawnTime);
            _waveText.text = "";
        }

        public void DecrementEnemiesRemaining()
        {
            EnemiesRemaining--;
        }

        public void DecrementCrashEnemiesNeedToSpawn()
        {
            CrashEnemiesNeedToSpawn--;
        }
    }
}