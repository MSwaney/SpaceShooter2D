using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _tractorBeamSlider;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Player _player;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _ammoCountText;
                     private GameManager _gameManager;

                     private bool _isGameOver;
                     private bool _canTractorBeam = true;
    
    [SerializeField] private float _tractorBeamSliderSpeed;

    [SerializeField] private int _tractorBeamSliderRefillSpeed;


    void Start()
    {
        _ammoCountText.text = "Ammo: " + 15;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null )
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoCountText.text = "Ammo: " + ammoCount.ToString();
        if (_ammoCountText.text == "Ammo: 0")
        {
            _ammoCountText.GetComponent<Text>().color = Color.red;
        }
        else
        {
            _ammoCountText.GetComponent <Text>().color = Color.white;
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence ()
    {
        _gameManager.GameOver();
        _isGameOver = true;
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (_isGameOver)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }      
    }

    public void UpdateTractorBeamSlider()
    {
        _tractorBeamSlider.GetComponent<Slider>().value -= _tractorBeamSliderSpeed;
        if (_tractorBeamSlider.GetComponent<Slider>().value == 1.0f)
        {
            _canTractorBeam = false;
            TractorBeamSliderFade();
            StartCoroutine(TractorBeamCooldown());
        }
    }

    public void TractorBeamSliderRefill()
    {
        float refillSpeed = _tractorBeamSliderSpeed;
        if (_canTractorBeam)
        {
            _tractorBeamSlider.GetComponent<Slider>().value += refillSpeed;
            TractorBeamSliderFade();
        }
    }

    private void TractorBeamSliderFade()
    {
        Image slider = _tractorBeamSlider.GetComponent<Slider>().GetComponentInChildren<Image>();
        if (slider != null && _canTractorBeam)
        {
            slider.color = new Color(slider.color.r, slider.color.g, slider.color.b, 1.5f);
        }
        else if (slider != null && !_canTractorBeam)
        {
            slider.color = new Color(slider.color.r, slider.color.g, slider.color.b, 0.5f);
        }
    }

    IEnumerator TractorBeamCooldown()
    {
        while (_canTractorBeam == false)
        {
            yield return new WaitForSeconds(_tractorBeamSliderRefillSpeed);
            _canTractorBeam = true;
        }
    }

    public bool CanTractorBeam()
    {
        return _canTractorBeam;
    }
}
