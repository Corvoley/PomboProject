using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private PlayerController player;
    [SerializeField] private float gameTimerCountdown;
    [SerializeField] private Text timeCounter;
    private bool isPaused = false;

    [SerializeField] private MusicPlayer musicPlayer;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        musicPlayer.PlayMainTrackMusic();

    }
    private void Update()
    {
        PauseHendler();
        GameTimeCounter();
    }

    private void PauseHendler()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
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

    public void PauseGame() 
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;
        player.enabled = false;
        isPaused = true;


    }
    public void ResumeGame()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;
        player.enabled = true;
        isPaused = false;
        
    }
}
