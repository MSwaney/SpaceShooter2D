using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HomingMissile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angleChangingSpeed;

                     private GameObject _closestEnemy;
                     
    [SerializeField] private Rigidbody2D _rigidBody;
                     private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _closestEnemy = CalculateClosestEnemy();
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_closestEnemy != null)
        {
            Vector2 direction = (Vector2)_closestEnemy.transform.position - _rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = transform.up * _speed;
        }
    }

    private GameObject CalculateClosestEnemy()
    {
        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");
        GameObject[] enemies3 = GameObject.FindGameObjectsWithTag("Enemy3");

        Transform playerTransform = _player.transform;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies1)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestEnemy = enemy;
            }
        }

        foreach (GameObject enemy in enemies2)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestEnemy = enemy;
            }
        }

        foreach (GameObject enemy in enemies3)
        {
            float distance = Vector3.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestEnemy = enemy;
            }
        }

        return _closestEnemy;
    }
}
