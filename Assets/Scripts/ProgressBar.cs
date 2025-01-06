using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI scoreCount;
    public TextMeshProUGUI finalScoreCount;
    public TextMeshProUGUI highScoreCount;

    public float drainSpeed = 5f;
    public float fillAmount = 20f;

    public float playerScore = 0f;
    public float comboMult = 1.0f;
    public float collectibleCount = 0f;

    public float highScore = 0f;
    public float finalScore = 0f;

    public UITimer uiTimer;

    void Start()
    {
        scoreCount.text = "score:   0";
        finalScoreCount.text = "score:   0";
        highScoreCount.text = "High score:   0";
    }

    void Update()
    {
        // Drain progress bar value over time
        if (progressBar.value > 0)
        {
            progressBar.value -= drainSpeed * Time.deltaTime;
        }

        else if (progressBar.value == progressBar.minValue)
        {
            comboText.text = "x1.0";
            collectibleCount = 0f;
            comboMult = 1.0f;
        }
    }

    public void CollectibleGrabbed()
    {
        // Increase player score based on the combo multiplier
        playerScore += 100 * comboMult;
        scoreCount.text = "score: " + playerScore.ToString();

        progressBar.value += fillAmount;

        if (progressBar.value > progressBar.maxValue)
        {
            progressBar.value = progressBar.maxValue;
        }

        collectibleCount++;

        if (collectibleCount >= 5 && collectibleCount <= 9)
        {
            comboText.text = "x1.5";
            comboMult = 1.5f;
        }
        else if (collectibleCount >= 10 && collectibleCount <= 14)
        {
            comboText.text = "x2.0";
            comboMult = 2.0f;
        }
    }

    public void OnPlayerDeath()
    {
        uiTimer.StopTimer();
        float finalTime = uiTimer.GetFinalTime();
        // Add the final time (rounded down) to the final score
        finalScore = Mathf.FloorToInt(finalTime) + playerScore;

        finalScoreCount.text = "score: " + finalScore.ToString();

        // Checks high scroe
        if (finalScore > highScore)
        {
            highScore = finalScore;
            highScoreCount.text = "High Score: " + highScore.ToString();
        }
    }
}
