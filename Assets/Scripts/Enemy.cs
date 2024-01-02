using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private bool _isDead = false;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        
        if ( _player == null )
        {
            Debug.LogError("Player on Enemy is NULL.");
        }

        if ( _animator == null )
        {
            Debug.LogError("Animator on Enemy is NULL.");
        }

        if (_audioSource == null )
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            float randomX = Random.Range(-15.5f, 15.5f);
            transform.position = new Vector3(randomX, 11f, 0f);
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
                GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.59f, 0), Quaternion.identity);
                laser.GetComponent<Laser>().AssignEnemyLaser();
            }
        }
    }

    private void DestroyEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0f;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        _isDead = true;
        Destroy(this.gameObject, 2.8f);
    }
}
