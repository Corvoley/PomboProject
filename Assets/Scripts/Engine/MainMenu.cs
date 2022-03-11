using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private int breadCount;
    [SerializeField] private int score;
    [SerializeField] private int health;
    [SerializeField] private Text finalScoreText;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name =="LastStage")
        {
            finalScoreText.text = $"Final Score:\n{PlayerPrefs.GetInt("Score").ToString()}";
        }
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("FirstStage");
        PlayerPrefs.SetInt("BreadCount", breadCount);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Health", health);

    }
    public void Menu()
    {

        SceneManager.LoadScene("MainMenu");
        
    }
    public void Credits()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
