using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemy2Prefab;
    [SerializeField] private GameObject _enemy3Prefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
                     private GameObject _powerup;
                     
    [SerializeField] private bool _stopSpawning = false;

                     private int _rarity;
    [SerializeField] private int _currentWave = 0;
    void Start()
    {
        //_rarity = GetComponent<Powerup>().ReturnRarity();
    }

    public void StartSpawning()
    {
        StartCoroutine(WaveRoutine());
        //StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        //StartCoroutine(SpawnEnemy2Routine());
    }

    IEnumerator WaveRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return StartCoroutine(SpawnEnemyWave(_currentWave));
            //yield return StartCoroutine(SpawnEnemy2Wave(_currentWave));

            yield return new WaitForSeconds(3f);

            _currentWave++;
            if (_currentWave == 5)
            {
                _stopSpawning = true;
                SpawnBoss();
            }
        }
    }

    private IEnumerator SpawnEnemyWave(int waveNumber)
    {
        int enemiesPerGroup = 5;
        int totalEnemies = (waveNumber + 1);

        for (int i = 0; i < totalEnemies; i++)
        {
            float enemyPrefabToSpawn = Random.Range(0f, 2f);

            if (enemyPrefabToSpawn > 0.8)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-14.5f, 14.5f), 11, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (enemyPrefabToSpawn > 0.3)
            {
                Vector3 posToSpawn = new Vector3(-16.7f, Random.Range(2.0f, 6.5f), 0);
                GameObject newEnemy = Instantiate(_enemy2Prefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-14.5f, 14.5f), 11, 0);
                GameObject newEnemy = Instantiate(_enemy3Prefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            if ((i + 1) % enemiesPerGroup == 0)
            {
                yield return new WaitForSeconds(3f);
            }
        }

        while (GameObject.FindWithTag("Enemy") != null)
        {
            yield return null;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            int totalRarity = 0;

            // Calculate total rarity to determine weight
            foreach (var powerup in _powerups)
            {
                totalRarity += powerup.GetComponent<Powerup>().ReturnRarity();
            }

            int randomWeight = Random.Range(0, totalRarity);
            int accumulatedWeight = 0;
            GameObject selectedPowerup = null;

            // Select a power-up based on weighted random selection
            foreach (var powerup in _powerups)
            {
                int rarity = powerup.GetComponent<Powerup>().ReturnRarity();
                accumulatedWeight += rarity;

                if (randomWeight < accumulatedWeight)
                {
                    selectedPowerup = powerup;
                    break;
                }
            }

            if (selectedPowerup != null)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-14.5f, 14.5f), 11, 0);
                Instantiate(selectedPowerup, posToSpawn, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private void SpawnBoss()
    {
        Vector3 posToSpawn = new Vector3(0.75f, 14.45f, 0f);
        GameObject newEnemy = Instantiate(_bossPrefab, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
    }
}
