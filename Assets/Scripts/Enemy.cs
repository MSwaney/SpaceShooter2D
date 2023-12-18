using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    private Animator _animator;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        if ( _player == null )
        {
            Debug.LogError("Player is NULL.");
        }

        if ( _animator == null )
        {
            Debug.LogError("Animator is NULL.");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0f);
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
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.8f);
        }
    }
}
