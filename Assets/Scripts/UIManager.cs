using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Text _ammoCountText;
    private GameManager _gameManager;

    private bool _isGameOver;

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
}
