using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.UI;

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
    private float _thrusterSpeed;
    [SerializeField]
    private float _thrusterBarDecreaseSpeed;
    [SerializeField]
    private float _thrusterBarIncreaseSpeed;
    [SerializeField]
    private float _thrusterCooldownDuration;
    [SerializeField]
    private float _thrusterCooldownTimer;


    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _multishotPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField] 
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _thrusterSlider;
    [SerializeField]
    private Transform _thrusterBar;
    [SerializeField]
    private Color _shieldTransparency;
    private CameraShake _cameraShake;


    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private int _shieldLives = 1;
    [SerializeField]
    private int _ammoCount;
    [SerializeField]
    private int _numberOfMultiShotLasers;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField] 
    private AudioClip _laserAudio;
    private AudioSource _audioSource;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _laserOnCooldown = false;
    private bool _thrusterOnCooldown = false;

    void Start()
    {
        _currentSpeed = _speed;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldTransparency = _shield.GetComponent<SpriteRenderer>().material.color;
        _thrusterBar = _thrusterSlider.transform.GetChild(1);
        _thrusterCooldownTimer = _thrusterCooldownDuration;
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is NULL.");
        }

        if (_cameraShake == null)
        {
            Debug.LogError("Camera on Player is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
        CalculateThrusterBar();

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

        if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterOnCooldown == false)
        {
            _currentSpeed *= _thrusterSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _currentSpeed /= _thrusterSpeed;
            if (_currentSpeed < _speed)
            {
                _currentSpeed = _speed;
            }
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
        if (_ammoCount <= 0 || _laserOnCooldown)
            return;

        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive && _ammoCount >= 3)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            SubtractAmmo(3);
        }
        else if (!_isTripleShotActive)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            SubtractAmmo(1);
        }

        _audioSource.clip = _laserAudio;
        _audioSource.Play();
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
            CameraShake();
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            CameraShake();
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            CameraShake();
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

    public void SubtractAmmo(int amount)
    {
        _ammoCount -= amount;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void AddAmmo()
    {
        if (_ammoCount < 15)
            _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void AddHealth()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
            {
                _leftEngine.SetActive(false);
            }
            else if (_lives == 3)
            {
                _rightEngine.SetActive(false);
            }
        }
    }

    public void FireMultiShot()
    {
        StartCoroutine(LaserCooldown());
        for (int i = 0; i < _numberOfMultiShotLasers; i++)
        {
            float angle = i * (360f / _numberOfMultiShotLasers);
            Quaternion laserRotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(_multishotPrefab, transform.position, laserRotation);
        }
    }

    private IEnumerator LaserCooldown()
    {
        _laserOnCooldown = true;
        yield return new WaitForSeconds(3);
        _laserOnCooldown = false;
    }

    private void CalculateThrusterBar()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_thrusterOnCooldown)
        {
            DecreaseThrusterBar();
        }
        else if (_thrusterOnCooldown || (!Input.GetKey(KeyCode.LeftShift) && _thrusterBar.transform.localScale.x < 1.0f))
        {
            if (_thrusterOnCooldown)
            {
                UpdateThrusterCooldown();
            }
            else
            {
                IncreaseThrusterBar();
            }
        }
    }

    private void IncreaseThrusterBar()
    {
        float thrusterPower = Mathf.Clamp01(_thrusterBar.transform.localScale.x + _thrusterBarIncreaseSpeed * Time.deltaTime);
        _thrusterBar.transform.localScale = new Vector3(thrusterPower, 1.0f, 1.0f);

        if (_thrusterBar.transform.localScale.x > 0.0f)
        {
            _thrusterCooldownTimer = _thrusterCooldownDuration;
        }
    }

    private void DecreaseThrusterBar()
    {
        float thrusterPower = Mathf.Clamp01(_thrusterBar.transform.localScale.x - _thrusterBarDecreaseSpeed * Time.deltaTime);
        _thrusterBar.transform.localScale = new Vector3(thrusterPower, 1.0f, 1.0f);

        if (_thrusterBar.transform.localScale.x == 0.0f)
        {
            _thrusterOnCooldown = true;
            _currentSpeed = _speed;
        }
    }

    private void UpdateThrusterCooldown()
    {
        _thrusterCooldownTimer -= Time.deltaTime;

        if (_thrusterCooldownTimer <= 0.0f)
        {
            _thrusterOnCooldown = false;
            _thrusterCooldownTimer = 0.0f;
        }
    }

    private void CameraShake()
    {
        if (_cameraShake != null)
        {
            Debug.Log("Made it to camera shake");
            _cameraShake.Shake();
        }
    }
}