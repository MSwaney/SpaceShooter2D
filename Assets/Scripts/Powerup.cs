using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField] // 0 = Triple shot 1 = Speed 2 = Shields
    private int powerupID;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GameObject.Find("Pickup").GetComponent<AudioSource>();

        if (_audioSource == null )
        {
            Debug.LogError("Audio Source on Powerup is NULL.");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

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
                    default:
                        Debug.Log("Defaul value.");
                        break;
                }
            }
            _audioSource.Play();
            Destroy(this.gameObject);
        }
    }
}
