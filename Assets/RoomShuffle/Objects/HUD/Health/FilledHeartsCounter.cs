using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FilledHeartsCounter : MonoBehaviour
{
    public TextMeshProUGUI PositivePop;
    public TextMeshProUGUI NegativePop;

    private TextMeshProUGUI TMP;
    private int _lastFilledHearts = 0;

    private void Start()
    {
        TMP = this.GetComponent<TextMeshProUGUI>();
    }

    public void setCounter(int number)
    {
        if (_lastFilledHearts == number)
        {
            return;
        }

        //Uncomment this if you want to show popping numbers!
        //int difference = number - _lastFilledHearts;
        //popNumber(difference);

        TMP.text = number.ToString();
        _lastFilledHearts = number;
    }

    public void popNumber(int number)
    {
        if (number > 0)
        {
            TextMeshProUGUI instance = Instantiate(
                original: PositivePop, 
                position: transform.position,
                rotation: Quaternion.identity,
                parent: transform
            );
                
            instance.text = "+" + number;
        }
        
        else if (number < 0)
        {
            TextMeshProUGUI instance = Instantiate(
                original: NegativePop, 
                position: transform.position,
                rotation: Quaternion.identity,
                parent: transform
            );
                
            instance.text = number.ToString();
        }
    }
}
