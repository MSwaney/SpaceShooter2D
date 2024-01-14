using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _zigzagAmplitude;
    [SerializeField]
    private float _zigzagSpeed;

    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shield;

    private bool _isDead = false;
    private bool _isShieldActive = false;

    void Start()
    {
        int shieldChance = Random.Range(1, 100);
        if (shieldChance % 20 == 0)
        {
            _isShieldActive = true;
            _shield.SetActive(true);
            _shield.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player on Enemy is NULL.");
        }

        if (_animator == null)
        {
            Debug.LogError("Animator on Enemy is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is NULL");
        }

        StartCoroutine(FireLaserRoutine());
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (tag == "Enemy")
        {
            float zigzagX = Mathf.PingPong(Time.time * _zigzagSpeed, _zigzagAmplitude * 2) - _zigzagAmplitude;

            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            transform.Translate(new Vector3(zigzagX, 0, 0) * Time.deltaTime);

            if (transform.position.y < -8f)
            {
                float randomX = Random.Range(-15.5f, 15.5f);
                transform.position = new Vector3(randomX, 11f, 0f);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -15.5f, 15f), transform.position.y, transform.position.z);
        }
        else if (tag == "Enemy2")
        {
            float zigzagY = Mathf.PingPong(Time.time * _zigzagSpeed, _zigzagAmplitude * 2) - _zigzagAmplitude;

            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            transform.Translate(new Vector3(0, zigzagY, 0) * Time.deltaTime);

            if (transform.position.x > 17.2)
            {
                float randomY = Random.Range(2f, 6.5f);
                transform.position = new Vector3(-17.2f, randomY, 0f);
            }

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 2f, 6.5f), transform.position.z);
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
            DestroyEnemy();
        }

        if (other.tag == "Laser")
        {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null && laser.IsEnemyLaser() == false)
            {
                if (_player != null)
                {
                    _player.AddToScore(10);
                }
                DestroyEnemy();
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            if (!_isDead)
            {
                GameObject player = GameObject.Find("Player");
                GameObject powerup = GameObject.FindGameObjectWithTag("Powerup");
                if (player.transform.position.y > transform.position.y)
                {
                    GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.59f, 0), Quaternion.identity);
                    laser.GetComponent<Laser>().AssignEnemyLaser();
                    laser.GetComponent<Laser>().FireUp();
                }
                else
                {
                    GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.59f, 0), Quaternion.identity);
                    laser.GetComponent<Laser>().AssignEnemyLaser();
                }

                if (powerup != null && IsPowerupBeneath(powerup))
                {
                    ShootPowerup(powerup);
                }
            }
        }
    }

    private void DestroyEnemy()
    {
        if (_isShieldActive == true)
        {
            _shield.SetActive(false);
            _isShieldActive = false;
            return;
        }
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0f;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        _isDead = true;
        Destroy(this.gameObject, 2.8f);
    }

    private bool IsPowerupBeneath(GameObject powerup)
    {
        float detectionThreshold = 5.0f;

        return Mathf.Abs(transform.position.y - powerup.transform.position.y) < detectionThreshold;
    }

    private void ShootPowerup(GameObject powerup)
    {
        if (powerup != null)
        {
            if (powerup.tag == "Powerup")
            {
                GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.59f, 0), Quaternion.identity);
                laser.GetComponent<Laser>().AssignEnemyLaser();
            }
        }
    }
}