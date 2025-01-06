using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    private bool isTimerActive = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isTimerActive)
        {
            timeElapsed += Time.deltaTime;

            // Sets time format to minutes:seconds
            int minutes = Mathf.FloorToInt(timeElapsed / 60);
            int seconds = Mathf.FloorToInt(timeElapsed % 60);

            // Updates the timerText with the formatted time
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        isTimerActive = true;
    }

    public void StopTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        isTimerActive = false;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public float GetFinalTime()
    {
        return timeElapsed;
    }
}
