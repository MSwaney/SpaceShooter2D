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
    private GameObject _powerup;

    private bool _stopSpawning = false;

    private int _rarity;

    void Start()
    {
        //_rarity = GetComponent<Powerup>().ReturnRarity();
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
}
