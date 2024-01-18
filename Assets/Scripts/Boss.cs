﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
                     private bool  _isDead;
    [SerializeField] private float _fireInterval;
    [SerializeField] private float _health;
                     private float _shrinkDuration = 4f;
    [SerializeField] private GameObject _doubleShotPrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
                     private Player _player;
                     private CameraShake _cameraShake;

    [Header("Movement Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _figureEightTime;
    [SerializeField] private float _figureEightRadius;

    [Header("Laser Fan Settings")]
    [SerializeField] private float _fanRadius;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _numOfFanLasers;

    [Header("Shotgun Settings")]
    [SerializeField] private float _shotgunSpeed;
    [SerializeField] private float _spreadAngle;
    [SerializeField] private int _numOfShotgunLasers;

    void Start()
    {
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(MoveOntoScreen());
    }

    private IEnumerator MoveOntoScreen()
    {
        Vector3 targetPostion = new Vector3(0.75f, 7f, 0f);
        while (transform.position.y > targetPostion.y)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(FigureEightMovement());
        StartCoroutine(FireLaserRoutine());
        StartCoroutine(LaserFanRoutine());
        StartCoroutine(ShotgunRoutine());
    }

    private IEnumerator FigureEightMovement()
    {
        float elapsedTime = 0f;
        Vector3 centerPosition = transform.position;

        while (!_isDead)
        {
            float x = centerPosition.x + Mathf.Sin(elapsedTime / _figureEightTime) * _figureEightRadius;
            float y = centerPosition.y - Mathf.Sin(elapsedTime * 2f / _figureEightTime) * (_figureEightRadius / 2f);

            transform.position = new Vector3(x, y, 0f);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
    }

    private IEnumerator FireLaserRoutine()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            if(!_isDead)
            {
                GameObject lasers = Instantiate(_doubleShotPrefab, transform.position + new Vector3(0, -3, 0), Quaternion.identity); 
                
                foreach (Transform laser in lasers.transform)
                {
                    laser.GetComponent<Laser>().AssignEnemyLaser();
                }
            }
        }
    }

    private IEnumerator LaserFanRoutine()
    {
        while (!_isDead)
        {
            for (int i = 0; i < _numOfFanLasers; i++)
            {
                float angle = i * -(180 / (_numOfFanLasers - 1));
                float radians = Mathf.Deg2Rad * angle;

                float x = _fanRadius * Mathf.Cos(radians);
                float y = _fanRadius * Mathf.Sin(radians);

                Vector3 laserPos = transform.position + new Vector3(x, y, 0) / 2;
                yield return new WaitForSeconds(0.1f);
                GameObject laser = Instantiate(_laserPrefab, laserPos, Quaternion.identity);

                float laserDirectionX = Mathf.Cos(radians);
                float laserDirectionY = Mathf.Sin(radians);
                Vector2 laserDirection = new Vector2(laserDirectionX, laserDirectionY).normalized;

                laser.GetComponent<Rigidbody2D>().velocity = laserDirection * _rotationSpeed;
                laser.GetComponent<Laser>().AssignBossLaser();

                yield return null;
            }

            yield return new WaitForSeconds(_fireInterval);
        }
    }

    private IEnumerator ShotgunRoutine()
    {
        while (!_isDead)
        {
            for (int i = 0; i < _numOfShotgunLasers; i++)
            {
                float angle = 0f;

                if (_numOfShotgunLasers > 1)
                {
                    angle = (i / (_numOfShotgunLasers - 1f) - 0.5f) * _spreadAngle;
                }

                float radians = Mathf.Deg2Rad * angle;
                float x = Mathf.Sin(radians);
                float y = -Mathf.Cos(radians); 

                Vector2 laserDirection = new Vector2(x, y).normalized;

                GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(-0.5f, -3f, 0), Quaternion.identity);

                laser.GetComponent<Rigidbody2D>().velocity = laserDirection * _shotgunSpeed;

                laser.GetComponent<Laser>().AssignBossLaser();
            }

            yield return new WaitForSeconds(_fireInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }

        if (other.tag == "Laser")
        {
            _health--;
            if (_health == 0)
            {
                DestroyBoss();
                _player.AddToScore(50);
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "Missile")
        {
            _health -= 3;
            if (_health <= 0)
            {
                Debug.Log(_health);
                DestroyBoss();
                _player.AddToScore(50);
            }
        }
    }

    private void DestroyBoss()
    {
        StopCoroutine(FigureEightMovement());
        StopCoroutine(FireLaserRoutine());
        StopCoroutine(LaserFanRoutine());
        StopCoroutine(ShotgunRoutine());
        StartCoroutine(BossExplosionsRoutine());
        StartCoroutine(MakeSmallerRoutine());
    }

    private IEnumerator BossExplosionsRoutine()
    {
        SpriteRenderer color = GetComponent<SpriteRenderer>();
        Instantiate(_explosionPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        _cameraShake.Shake();
        color.color = new Color(color.color.r, color.color.g, color.color.b, 0.75f);
        yield return new WaitForSeconds(1);
        Instantiate(_explosionPrefab, transform.position + new Vector3(-1f, 0f, 0f), Quaternion.identity);
        _cameraShake.Shake();
        color.color = new Color(color.color.r, color.color.g, color.color.b, 0.5f);
        yield return new WaitForSeconds(1);
        Instantiate(_explosionPrefab, transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity);
        _cameraShake.Shake();
        color.color = new Color(color.color.r, color.color.g, color.color.b, 0.25f);
        yield return new WaitForSeconds(1);
        Instantiate(_explosionPrefab, transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
        _cameraShake.Shake();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator MakeSmallerRoutine()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(0.0f, 0.0f, 0.0f);

        float elapsedTime = 0f;

        while (elapsedTime < _shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / _shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
