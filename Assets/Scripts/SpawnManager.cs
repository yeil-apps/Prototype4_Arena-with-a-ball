using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private GameObject[] _powerupPrefabs;

    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject[] _miniEnemyPrefabs;
    [SerializeField] private int _bossRound;

    private float _spawnRange = 9f;
    private int _enemyCount;
    private int _waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(_waveNumber);
        SpawnRandomPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        _enemyCount = FindObjectsOfType<Enemy>().Length;

        if (_enemyCount == 0)
        {
            _waveNumber++;
            SpawnRandomPowerup();
            if (_waveNumber % _bossRound == 0)
            {
                SpawnBossWave(_waveNumber);
            }
            else
            {
                SpawnEnemyWave(_waveNumber);
            }
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, _enemyPrefabs.Length);
            Instantiate(_enemyPrefabs[enemyIndex], GenerateSpawnPosition(), _enemyPrefabs[enemyIndex].transform.rotation);
        }
    }

    private void SpawnRandomPowerup()
    {
        int powerupIndex = Random.Range(0, _powerupPrefabs.Length);
        Instantiate(_powerupPrefabs[powerupIndex], GenerateSpawnPosition(), _powerupPrefabs[powerupIndex].transform.rotation);
    }

    private void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
        if (_bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / _bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }

        var boss = Instantiate(_bossPrefab, GenerateSpawnPosition(), _bossPrefab.transform.rotation);
        boss.GetComponent<Enemy>().MiniEnemySpawnCount = miniEnemysToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i =0; i< amount; i++)
        {
            int randomMiniEmeny = Random.Range(0, _miniEnemyPrefabs.Length);
            Instantiate(_miniEnemyPrefabs[randomMiniEmeny], GenerateSpawnPosition(), _miniEnemyPrefabs[randomMiniEmeny].transform.rotation);
        }
    }
}
