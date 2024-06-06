using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int CoinsCollected = 0;
    public int DamageLevel = 1;
    public Text Score;
    public Text timer; 

    private float elapsedTime = 0f;
    private bool isTiming = true;

    public void Awake()
    {
        // Check if the user is logged in else redirect to the login scene
        if (DBmanager.username == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            CoinsCollected = DBmanager.coins;
            DamageLevel = DBmanager.damage;
        }
    }

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        // Update the score display
        Score.text = CoinsCollected.ToString();

        // Update the timer if running
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        // convert time
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // text ui verversen
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    public void StartTimer()
    {
        isTiming = true;
        elapsedTime = 0f;
    }

    public void CompleteLevel()
    {
        StopTimer();
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void CollectCoins()
    {
        CoinsCollected++;
    }

    public void CollectScore()
    {
        GetComponent<Text>().text = Score.text;
    }
}
