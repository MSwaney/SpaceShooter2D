using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser;
    [SerializeField]
    private float _enemyLaserSpeed;


    void Update()
    {
        CalculateMovement();

        if (transform.position.y > 11.0f || transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        else if (transform.position.x > 16.5f || transform.position.x < -16.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void CalculateMovement()
    {
        if (_isEnemyLaser)
        {
            transform.Translate(Vector3.down * _speed* _enemyLaserSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public bool IsEnemyLaser()
    {
        return _isEnemyLaser;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyLaser)
        {
            if(other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
