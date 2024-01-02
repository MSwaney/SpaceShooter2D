using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-14.5f, 14.5f), 11, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            int powerupID = Random.Range(0, _powerups.Length);
            Vector3 posToSpawn = new Vector3(Random.Range(-14.5f, 14.5f), 11, 0);
            int randomPowerUp = Random.Range(0, _powerups.Length);
            if (randomPowerUp == 5)
            {
                int chance = Random.Range(0, 101);
                if (chance % 10  == 0)
                {
                    Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
                }
            } 
            else
            {
                Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
