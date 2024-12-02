using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    float startTime;
    int minutes, seconds;

    bool isRunning = true;

    private void Start()
    {
        timerText.color = Color.green;
        startTime = remainingTime;
    }

    private void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0f)
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime < startTime/4)
                {
                    timerText.color = Color.red;
                }
                else if (remainingTime < startTime / 2)
                {
                    timerText.color = Color.yellow;
                }
            }
            else
            {
                remainingTime = 0f;
                ArenaController.Instance.TimeOut();
            }
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        minutes = Mathf.FloorToInt(remainingTime / 60);
        seconds = Mathf.CeilToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TimerRunning(bool state)
    {
        isRunning = state;
    }

    public void RestartTimer()
    {
        timerText.color = Color.green;

        remainingTime = startTime;
        UpdateTimerText();
    }
}
