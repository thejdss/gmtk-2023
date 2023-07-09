using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public TextMeshProUGUI levelText;

    public BoardController boardController;
    public int things = 1;
    public TextMeshProUGUI thingsText;

    public TextMeshProUGUI timeText;
    public float timeRemaining = 160;
    public bool timerIsRunning;

    private void Start()
    {
        StartLevel();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void StartLevel()
    {
        timeRemaining = 30;
        things = 10;
        thingsText.text = things.ToString();

        level += 1;
        levelText.text = level.ToString();

        boardController.RegisterOnHold(DecreaseThings);
        timerIsRunning = true;
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
            // game over
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
