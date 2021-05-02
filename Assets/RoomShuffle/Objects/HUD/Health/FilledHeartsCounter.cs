using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FilledHeartsCounter : MonoBehaviour
{
    private TextMeshProUGUI TMP;
    private int _lastFilledHearts;

    private void Start()
    {
        TMP = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Sets the heartcounter count
    /// </summary>
    /// <param name="number"></param>
    public void SetCounter(int number)
    {
        //if theres has been no chenges: return
        if (_lastFilledHearts == number)
        {
            return;
        }
        
        //sets text
        TMP.text = number + "+";
        
        //set last state
        _lastFilledHearts = number;
    }
}
