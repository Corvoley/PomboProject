using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private int breadCount;
    [SerializeField] private int score;
    [SerializeField] private int health;

    public void StartGame()
    {
        SceneManager.LoadScene("FirstStage");
        PlayerPrefs.SetInt("BreadCount", breadCount);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Health", health);

    }
}
