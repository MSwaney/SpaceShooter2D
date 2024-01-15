using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
   
    [SerializeField]
    private int powerupID; // 0 = Triple shot 1 = Speed 2 = Shields 3 = Ammo 4 = Health 5 = Multishot 6 = Thruster Debuff
    [SerializeField]
    private int _rarity;

    private bool _isDebuff = false;

    private AudioSource _pickup;
    private AudioSource _debuff;

    private void Start()
    {
        _pickup = GameObject.Find("Pickup").GetComponent<AudioSource>();
        _debuff = GameObject.Find("Debuff").GetComponent<AudioSource>();

        if (_pickup == null || _debuff == null)
        {
            Debug.LogError("Audio Source on Powerup is NULL.");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }

    }

    public int ReturnRarity()
    {
        return _rarity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.AddHealth();
                        break;
                    case 5:
                        player.FireMultiShot();
                        break;
                    case 6:
                        _isDebuff = true;
                        player.ThrusterDebuff();
                        break;
                    case 7:
                        player.AddMissile();
                        break;
                    default:
                        Debug.Log("Default value.");
                        break;
                }
            }
            if (_isDebuff == false)
            {
                _pickup.Play();
            }
            else
            {
                _debuff.Play();
                _isDebuff = false;
            }
            PowerupDestroy();
        }
    }

    public void PowerupDestroy()
    {
        Destroy(this.gameObject);
    }
}
