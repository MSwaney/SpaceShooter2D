using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");
        GameObject[] enemies3 = GameObject.FindGameObjectsWithTag("Enemy3");

        Transform playerTransform = _player.transform;
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies1)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        foreach (GameObject enemy in enemies2)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        foreach (GameObject enemy in enemies3)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Debug.Log(closestEnemy.ToString());
        }
    }
}
