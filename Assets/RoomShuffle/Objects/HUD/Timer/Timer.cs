using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    [Tooltip("The number that represents the added time on time added")]
    public TextMeshProUGUI PositivePopNumber;
    
    [Tooltip("The number that represents the added time on time subtracted")]
    public TextMeshProUGUI NegativePopNumber;

    [Tooltip("How much time is left before the text starts blinking and ticking")]
    public float BlinkingTime = 10;

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
        
        //TODO remove this counter
        StartCountdown(60);
    }

    //TODO Remove this nonsense in update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddTime(10);
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            SubtractTime(5);
        }
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
    /// Adds time to the Timer or Countdown
    /// </summary>
    public void AddTime(float seconds)
    {
        CurrentSeconds += seconds;

        if (PositivePopNumber)
        {
            TextMeshProUGUI instance = Instantiate(
                original: PositivePopNumber, 
                position: transform.position,
                rotation: Quaternion.identity,
                parent: transform
            );
                
            instance.text = "+" + seconds;
        }
    }
    
    /// <summary>
    /// Adds time to the Timer or Countdown
    /// </summary>
    public void SubtractTime(float seconds)
    {
        CurrentSeconds -= seconds;

        if (NegativePopNumber)
        {
            TextMeshProUGUI instance = Instantiate(
                original: NegativePopNumber, 
                position: transform.position,
                rotation: Quaternion.identity,
                parent: transform
            );
                
            instance.text = "-" + seconds;
        }
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
        TMP.color = Color.white;
        transform.localScale = Vector3.one * 1f;
        CurrentSeconds = seconds;
        bool blinking = false;

        while (CurrentSeconds > 0)
        {
            TMP.text = formatNumber(CurrentSeconds);
            CurrentSeconds -= Time.deltaTime;

            //Should the timer start blinking
            if (CurrentSeconds < BlinkingTime)
            {
                if (!blinking)
                {
                    StartCoroutine(CoStartBlinking());
                    blinking = true;
                }
            }
            else
            {
                TMP.color = Color.white;
                transform.localScale = Vector3.one * 1f;
                blinking = false;
            }

            yield return new WaitForEndOfFrame();
        }

        CurrentSeconds = 0;
        TMP.text = formatNumber(CurrentSeconds);
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

    
    private IEnumerator CoStartBlinking()
    {
        transform.localScale = Vector3.one * 1.5f;
        
        bool alternate = true;

        //While the timer is between 0 and max blinking time
        while ( CurrentSeconds > 0 && CurrentSeconds < BlinkingTime)
        {
            if (alternate)
            {
                TMP.color = Color.red;
                //TODO Tick
            }
            else
            {
                TMP.color = Color.white;
                //TODO Tock
            }

            yield return new WaitForSeconds((CurrentSeconds < 1.5f ? 1.5f : CurrentSeconds)/BlinkingTime);
            alternate = !alternate;
        }
        
        TMP.color = Color.red;
        
        transform.localScale = Vector3.one * 1f;
    }
    
}
