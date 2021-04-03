using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{

    /// <summary>
    /// The displayed timer value in seconds
    /// </summary>
    public float CurrentSeconds { get; set; }
    
    //if the timer is running
    private bool _timerIsRunning = false;
    
    private float _countDownTarget;

    private TextMeshProUGUI TMP;
    
    public float TimeLeft
    {
        get
        {
            return _countDownTarget - CurrentSeconds;
        }
    }

    private void Start()
    {
        //Get textmeshpro component
        TMP = GetComponent<TextMeshProUGUI>();
        StartTimer();
    }

    /// <summary>
    /// Starts a timer of the timer isn't running
    /// </summary>
    public void StartTimer()
    {
        if (_timerIsRunning)
            return;
            
        StartCoroutine(CoStartTimer());
        _timerIsRunning = true;
    }
    
    /// <summary>
    /// Starts the timer from zero even if the timer is running
    /// </summary>
    public void ResetTimer()
    {
        StopTimer();
        StartTimer();
    }

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void StopTimer()
    {
        StopAllCoroutines();
        _timerIsRunning = false;
    }
    
    /// <summary>
    /// Starts a countdown if the countdown isn't already running
    /// </summary>
    /// <param name="seconds"></param>
    public void StartCountdown(float seconds)
    {
        if (_timerIsRunning)
            return;
            
        StartCoroutine(CoStartCountdown(seconds));
        _timerIsRunning = true;
    }
    
    /// <summary>
    /// Starts a countdown even if the countdown is running
    /// </summary>
    /// <param name="seconds"></param>
    public void ResetCountdown(float seconds)
    {
        StopCountdown();
        StartCountdown(seconds);
    }

    /// <summary>
    /// Stops the countdown
    /// </summary>
    public void StopCountdown()
    {
        StopAllCoroutines();
        _timerIsRunning = false;
    }

    /// <summary>
    /// Timer coroutine adding deltatime to the secounds variable
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoStartTimer()
    {
        while (true)
        {
            TMP.text = formatNumber(CurrentSeconds);
            CurrentSeconds += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
    
    /// <summary>
    /// Countdown coroutine subtracting deltatime from the secounds variable
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoStartCountdown(float seconds)
    {
        CurrentSeconds = seconds;

        while (CurrentSeconds > 0)
        {
            TMP.text = formatNumber(CurrentSeconds);
            CurrentSeconds -= Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }

        TMP.text = formatNumber(0);
    }

    private string formatNumber(float number)
    {
        TimeSpan t = TimeSpan.FromSeconds(number);

        string formattedString = string.Format("{0:D2}{1:D2}{2:D2}.{3:D2}", 
            (int)t.TotalHours == 0 ? "" : ((int)t.TotalHours).ToString("D2") + ":", 
            t.Minutes == 0 ? "" : t.Minutes.ToString("D2") + ":", 
            t.Seconds, 
            t.Milliseconds / 10);

        return formattedString;

    }
}
