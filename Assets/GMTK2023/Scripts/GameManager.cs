using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool CanPlay;
    public GameAttributes attributes;
    public Animator animatorPeoples;

    public GameObject gameOverScreen;
    public int currentLevel = 1;
    public TextMeshProUGUI levelText;

    public BoardController boardController;
    private int things = 1;
    public TextMeshProUGUI thingsText;

    private int cpuPoints = 0;
    public TextMeshProUGUI cpuPointsText;
    public CPUController cpuController;

    public TextMeshProUGUI timeText;
    private float timeRemaining = 160;
    private bool timerIsRunning;

    private int startThings;
    private float startTime;

    private void Start()
    {
        cpuController.RegisterHammerCheckCollision(IncreaseCPUPoints);
        boardController.RegisterOnHold(DecreaseThings);

        CanPlay = true;
        currentLevel = -1;

        startTime = attributes.timeRemaining;
        startThings = attributes.things;
    }

    private void Update()
    {
        UpdateTimer();
        UpdateThings();
    }

    public void StartLevel()
    {
        timeRemaining = startTime = startTime - 10;
        things = startThings = startThings + 2;
        thingsText.text = things.ToString();

        currentLevel += 1;
        levelText.text = currentLevel.ToString();

        cpuPoints = 0;
        cpuPointsText.text = cpuPoints.ToString();

        cpuController.SetHammerAttributes();

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
        Debug.Log("Play " + things);
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
                FinishLevel();
                timerIsRunning = false;
            }
        }
    }

    private void UpdateThings()
    {
        if (things <= 0)
        {
            timeRemaining = 0;
            timerIsRunning = false;
            StartCoroutine(StartNewLevel());
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
            StartCoroutine(StartNewLevel());
        }
    }

    private IEnumerator StartNewLevel()
    {
        CanPlay = false;
        things = 1;
        int rand = Random.Range(0, 5);
        animatorPeoples.SetInteger("PeopleNumber", rand);
        animatorPeoples.SetTrigger("End");
        yield return new WaitForSeconds(3.5f);
        StartLevel();
        CanPlay = true;
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeText.text = timeToDisplay.ToString("00");
    }
}

[System.Serializable]
public class GameAttributes
{
    public int things = 2;
    public float timeRemaining = 160;
}
