using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private bool _fireUp = false;
    private bool _isBossLaser;
    private bool _isEnemyLaser;
    
    [SerializeField] private float _enemyLaserSpeed;
    [SerializeField] private float _speed = 8f;


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
        if (!_isBossLaser)
        {
            if (_isEnemyLaser && !_fireUp)
            {
                transform.Translate(Vector3.down * _speed * _enemyLaserSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }
        }
    }

    public void AssignEnemyLaser()
    {
        this.tag = "EnemyLaser";
        _isEnemyLaser = true;
    }

    public bool IsEnemyLaser()
    {
        return _isEnemyLaser;       
    }
    
    public void AssignBossLaser()
    {
        this.tag = "BossLaser";
        _isBossLaser = true;
    }

    public bool IsBossLaser() 
    { 
        return _isBossLaser; 
    }

    public void FireUp()
    {
        _fireUp = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyLaser || _isBossLaser)
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
            else if (other.tag == "Powerup")
            {
                Powerup powerup = other.GetComponent<Powerup>();
                if (powerup != null)
                {
                    powerup.PowerupDestroy();
                }
            }
        }
    }
}
