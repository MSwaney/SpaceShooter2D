using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;
    [SerializeField]
    private float _currentSpeed;
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private float _speedBoost;
   

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField] 
    private GameObject _rightEngine;
    private Color _shieldTransparency;


    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private int _shieldLives = 1;
    [SerializeField]
    private int _ammoCount;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField] private AudioClip _laserAudio;
    private AudioSource _audioSource;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    void Start()
    {
        _currentSpeed = _speed;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldTransparency = _shield.GetComponent<SpriteRenderer>().material.color;

        if (_spawnManager == null )
        {
            Debug.LogError("Spawn Manager is NULL.");
        }
        
        if (_uiManager == null )
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _currentSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            _currentSpeed *= _speedBoost;
        } 
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentSpeed /= _speedBoost;
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -6.5f)
        {
            transform.position = new Vector3(transform.position.x, -6.5f, 0);
        }

        if (transform.position.x > 16.5f)
        {
            transform.position = new Vector3(-16.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -16.5f)
        {
            transform.position = new Vector3(16.5f, transform.position.y, 0);
        }
    }
    
    void FireLaser()
    {
        if (_ammoCount > 0)
        {
            _canFire = Time.time + _fireRate;
        
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity) ;
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audioSource.clip = _laserAudio;
            _audioSource.Play();
            SubractAmmo();
        }
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            if (_shieldLives == 1)
            {
                _shieldLives -= 1;
                _shield.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            } 
            else if (_shieldLives == 0)
            {
                _isShieldActive = false;
                _shield.SetActive(false);
            }
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);   
        }
        else if ( _lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        if (_isSpeedBoostActive == false)
        {
            _currentSpeed *= _speedMultiplier;
            _isSpeedBoostActive = true;
        }
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _currentSpeed /= _speedMultiplier;
        if (_currentSpeed < _speed)
        {
            _currentSpeed = _speed;
        }
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
        _shieldLives = 1;
        _shield.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void SubractAmmo()
    {
        _ammoCount -= 1;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void AddAmmo()
    {
        _ammoCount += 10;
        _uiManager.UpdateAmmo(_ammoCount);
    }
}
