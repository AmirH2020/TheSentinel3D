using System.Collections.Generic;
using TheSentinel.Cores;
using UnityEngine;
using TMPro;
using System.Collections;


namespace TheSentinel
{
    [System.Serializable]
    public class SpawnBoundaries<T>
    {
        public T _lowerZ, _upperZ, _upperX, _lowerX;
    }
    public class SpawnerScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text _waveText;

        [SerializeField] private List<GameObject> _enemies;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _initialSpawnTime;
        [SerializeField] private SpawnBoundaries<Transform> _spawnBoundaries;
        public static WaveManager _enemyWaveManager { get; private set; }
        private EnemySpawner _enemySpawner;
        private float lastUpgradeStamp;
        private int _level;

        private void Start()
        {
            _enemyWaveManager = new WaveManager(_waveText,this,_spawnTime);

            _enemySpawner = new EnemySpawner(_enemies, _spawnTime, _initialSpawnTime, _enemyWaveManager, _spawnBoundaries);

            StartCoroutine(_enemyWaveManager.WaveText());
        }
        private void Update()
        {
            if (GameManager.OnPause) return;
            
            _enemyWaveManager.UpdateWaveTimer(Time.deltaTime);
            if (Time.time - lastUpgradeStamp >= 25)
            {
                lastUpgradeStamp = Time.time;
                _level++;
            }

            _enemyWaveManager.HandleWaveProgress();

            _enemySpawner.UpdateSpawner(Time.deltaTime, _level);

        }


    }
}