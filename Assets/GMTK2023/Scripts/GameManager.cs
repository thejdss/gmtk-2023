using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public int level = 1;
    public TextMeshProUGUI levelText;

    public BoardController boardController;
    public int things = 1;
    public TextMeshProUGUI thingsText;

    public int cpuPoints;
    public TextMeshProUGUI cpuPointsText;
    public CPUController cpuController;

    public TextMeshProUGUI timeText;
    public float timeRemaining = 160;
    public bool timerIsRunning;

    private void Start()
    {
        cpuController.RegisterHammerCheckCollision(IncreaseCPUPoints);
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void StartLevel()
    {
        timeRemaining = 30;
        things = 10;
        thingsText.text = things.ToString();

        level += 1;
        levelText.text = level.ToString();

        cpuPoints += 1;
        cpuPointsText.text = cpuPoints.ToString();

        boardController.RegisterOnHold(DecreaseThings);
        timerIsRunning = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void IncreaseCPUPoints()
    {
        cpuPoints += 10;
        cpuPointsText.text = cpuPoints.ToString();
    }

    private void DecreaseThings()
    {
        things -= 1;
        thingsText.text = things.ToString();
    }

    private void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                FinishLevel();
            }
        }
    }

    private void FinishLevel()
    {
        if (things > 0)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            StartLevel();
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeText.text = timeToDisplay.ToString("00");
    }
}
