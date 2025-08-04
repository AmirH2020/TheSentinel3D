using System.Collections.Generic;
using UnityEngine;


namespace TheSentinel
{
    public class EnemySpawner
    {
        private readonly WaveManager _waveManager;
        private readonly float _spawnTime;
        private readonly List<GameObject> _enemies;

        private float _spawnTimer;

        private SpawnBoundaries<float> _spawnBoundaries;

        public EnemySpawner(List<GameObject> enemies, float spawnTime,float initialSpawnTime, WaveManager waveManager,SpawnBoundaries<Transform> spawnBoundaries)
        {
            _spawnBoundaries = new SpawnBoundaries<float>();
            _enemies = enemies;
            _spawnTime = spawnTime;
            _spawnTimer = initialSpawnTime;
            _waveManager = waveManager;
            CalculateSpawnBoundaries(spawnBoundaries);
        }
        public void UpdateSpawner(float deltaTime,int level)
        {
            _spawnTimer -= deltaTime;
            _spawnTimer = Mathf.Max(0, _spawnTimer);

            if (_spawnTimer <= 0 && _waveManager.EnemiesRemaining > 0)
                SpawnEnemies(level);
        }
        private void SpawnEnemies(int level)
        {
            int temp = Mathf.Min(_waveManager.Wave + 1, 3);
            int enemiesSpawnedAtOnce = Random.Range(1, temp);

            for (int i = 0; i < enemiesSpawnedAtOnce; i++)
            {
                int enemyType = Random.Range(0, 2);
                GameObject enemyPrefab = (_waveManager.CrashEnemiesNeedToSpawn > 0 && enemyType == 1) ? _enemies[1] : _enemies[0];
                Spawn(enemyPrefab,level);
            }
            _spawnTimer = _spawnTime;
        }
        private void Spawn(GameObject enemyPrefab,int level)
        {

            var pos = GetRandomPos();
            pos.y = 1;
            var enemy = Object.Instantiate(enemyPrefab, pos, Quaternion.identity);

            if (enemy.TryGetComponent<EnemyGunner>(out var enemyGunner))
                enemyGunner.AssignLevelStats(level);
            else
                _waveManager.DecrementCrashEnemiesNeedToSpawn();
            _waveManager.DecrementEnemiesRemaining();
        }
        private void CalculateSpawnBoundaries(SpawnBoundaries<Transform> spawnBoundaries)
        {
            _spawnBoundaries._upperZ = spawnBoundaries._upperZ.position.z;
            _spawnBoundaries._lowerZ = spawnBoundaries._lowerZ.position.z;
            _spawnBoundaries._upperX = spawnBoundaries._upperX.position.x;
            _spawnBoundaries._lowerX = spawnBoundaries._lowerX.position.x;
        }
        public Vector3 GetRandomPos()
        {
            return Random.Range(0, 4) switch
            {
                0 => new Vector3(Random.Range(_spawnBoundaries._upperX, _spawnBoundaries._lowerX), 0, _spawnBoundaries._upperZ),
                1 => new Vector3(Random.Range(_spawnBoundaries._upperX, _spawnBoundaries._lowerX), 0, _spawnBoundaries._lowerZ),
                2 => new Vector3(_spawnBoundaries._upperX, 0, Random.Range(_spawnBoundaries._upperZ, _spawnBoundaries._lowerZ)),
                _ => new Vector3(_spawnBoundaries._lowerX, 0, Random.Range(_spawnBoundaries._upperZ, _spawnBoundaries._lowerZ))
            };
        }
    }
}