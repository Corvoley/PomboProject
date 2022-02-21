using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverImage;
    private PlayerController player;
    [SerializeField] private float gameTimerCountdown;
    [SerializeField] private Text timeCounter;
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        
    }
    private void Update()
    {
        GameTimeCounter();
    }
    public void EndGame()
    {
        Time.timeScale = 0;
        gameOverImage.SetActive(true);
        player.enabled = false;
    }

    public void RestartGame()
    {
        gameOverImage.SetActive(false);
        Time.timeScale = 1;        
        SceneManager.LoadScene("MainMenu");
        player.enabled = true;
    }


    private void GameTimeCounter()
    {
        if (gameTimerCountdown <= 0)
        {
            EndGame();
        }
        gameTimerCountdown -= Time.deltaTime;
        timeCounter.text = Mathf.Round(gameTimerCountdown).ToString();       
    }

}
