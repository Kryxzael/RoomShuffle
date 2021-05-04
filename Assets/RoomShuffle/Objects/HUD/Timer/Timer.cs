using System;
using System.Collections;

using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Tooltip("The number that represents the added time on time added")]
    public TextMeshProUGUI PositivePopNumber;
    
    [Tooltip("The number that represents the added time on time subtracted")]
    public TextMeshProUGUI NegativePopNumber;

    [Tooltip("How much time is left before the text starts blinking and ticking")]
    public float BlinkingTime = 10;

    [Tooltip("The formating string to use when displaying the time. This does not decide the format of the time itself but the text around it")]
    public string TimerTextFormat = "{0}";

    /// <summary>
    /// The displayed timer value in seconds
    /// </summary>
    public float CurrentSeconds { get; set; }
    
    //if the timer is running
    public bool TimerIsRunning { get; private set; } = false;

    private TextMeshProUGUI TMP_GUI;

    private TextMeshPro TMP;

    private bool UseGUI;

    private MultiSoundPlayer _multiSoundPlayer;

    private void Start()
    {
        
        //Get audio source. This is only in use when "UseGUI is false"
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();

        //Get textmeshpro component
        TMP_GUI = GetComponent<TextMeshProUGUI>();

        if (TMP_GUI)
        {
            UseGUI = true;
            return;
        }

        TMP = GetComponent<TextMeshPro>();
        
    }

    /// <summary>
    /// Starts a stopwatch if the timer isn't running
    /// </summary>
    public void StartStopwatch()
    {
        if (TimerIsRunning)
            return;
            
        StartCoroutine(CoStartTimer());
        TimerIsRunning = true;
    }
    
    /// <summary>
    /// Starts the stopwatch from zero even if the timer is running
    /// </summary>
    public void ResetStopwatch()
    {
        StopTimer();
        StartStopwatch();
    }

    /// <summary>
    /// Stops the countdown/stopwatch
    /// </summary>
    public void StopTimer()
    {
        StopAllCoroutines();
        TimerIsRunning = false;
    }
    
    /// <summary>
    /// Starts a countdown if the countdown isn't already running
    /// </summary>
    /// <param name="seconds"></param>
    public void StartCountdown(float seconds)
    {
        if (TimerIsRunning)
            return;
        
        StartCoroutine(CoStartCountdown(seconds));
        TimerIsRunning = true;
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
        TimerIsRunning = false;
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
            SetText(FormatNumber(CurrentSeconds));
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

        if (!UseGUI)
        { 
            StartCoroutine(MakeTickNoise());
        }

        SetColor(Color.white);
        
        transform.localScale = UseGUI ? Vector3.one * 1f : transform.localScale;
        bool blinking = false;

        while (CurrentSeconds > 0)
        {
            SetText(FormatNumber(CurrentSeconds));
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
                SetColor(Color.black);
                
                if (UseGUI)
                    transform.localScale = Vector3.one * 1f;
                
                blinking = false;
            }

            yield return new WaitForEndOfFrame();
        }

        CurrentSeconds = 0;
        SetText(FormatNumber(CurrentSeconds));
    }

    private string FormatNumber(float number)
    {
        TimeSpan t = TimeSpan.FromSeconds(number);

        string formattedString = string.Format("{0:D2}{1:D2}{2:D2}.{3:D2}", 
            (int)t.TotalHours == 0 ? "" : ((int)t.TotalHours).ToString("D2") + ":", 
            t.Minutes == 0 ? "" : t.Minutes.ToString("D2") + ":", 
            t.Seconds, 
            t.Milliseconds / 10);

        return string.Format(TimerTextFormat, arg0: formattedString);

    }


    private IEnumerator CoStartBlinking()
    {
        if (UseGUI)
            transform.localScale = Vector3.one * 1.5f;
        
        bool alternate = true;

        //While the timer is between 0 and max blinking time
        while ( CurrentSeconds > 0 && CurrentSeconds < BlinkingTime)
        {
            if (alternate)
            {
                SetColor(Color.red);
            }
            else
            {
                SetColor(Color.black);
            }

            yield return new WaitForSeconds((CurrentSeconds < 1.5f ? 1.5f : CurrentSeconds)/BlinkingTime);
            alternate = !alternate;
        }
        
        SetColor(Color.red);
        
        if (UseGUI)
            transform.localScale = Vector3.one * 1;
    }

    public void HideTimer()
    {
        StopTimer();
        SetText("");
    }

    /// <summary>
    /// Sets the text of the textmeshpro
    /// </summary>
    /// <param name="text"></param>
    private void SetText(string text)
    {

        if (UseGUI)
        {
            if (!TMP_GUI)
            {
                Debug.Log("TMP_GUI is not set");
                return;
            }
            TMP_GUI.text = text;
            return;
        }

        if (!TMP)
        {
            Debug.Log("TMP is not set");
            return;
        }

        TMP.text = text;
    }
    
    /// <summary>
    /// Sets the color of the Textmeshpro
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        if (UseGUI)
        {
            TMP_GUI.color = color;
            return;
        }

        TMP.color = color;
    }

    private IEnumerator MakeTickNoise()
    {

        while (CurrentSeconds > 0 && CurrentSeconds > BlinkingTime)
        {
            _multiSoundPlayer.PlaySound(0);
            yield return new WaitForSeconds(0.5f);
            _multiSoundPlayer.PlaySound(1);
            yield return new WaitForSeconds(0.5f);
        }
        while (CurrentSeconds > 0 && CurrentSeconds > BlinkingTime/2)
        {
            _multiSoundPlayer.PlaySound(0);
            yield return new WaitForSeconds(0.25f);
            _multiSoundPlayer.PlaySound(1);
            yield return new WaitForSeconds(0.25f);
        }
        while (CurrentSeconds > 0)
        {
            _multiSoundPlayer.PlaySound(0);
            yield return new WaitForSeconds(0.15f);
            _multiSoundPlayer.PlaySound(1);
            yield return new WaitForSeconds(0.15f);
        }
    }

}
