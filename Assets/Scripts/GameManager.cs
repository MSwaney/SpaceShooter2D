﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed");

            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
