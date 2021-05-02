using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    private List<Button> _buttonList = new List<Button>();
    private GameObject _enableOnPressed;
    private GameObject _disableOnPressed;
    private GameObject _enableForeverOnPressed;
    private GameObject _disableForeverOnPressed;

    private bool _firstPress = true;
    private bool _lastState = false;
    void Start()
    {
        
        //get all children in button group
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Buttons"))
            {
                //if the child is called button, add to button group
                child.Cast<Transform>().ForEach(x => _buttonList.Add(x.GetComponent<Button>()));
                continue;
            }

            //Set fields to children
            switch (child.name)
            {
                case "EnableOnPressed":
                    _enableOnPressed = child.gameObject;
                    break;
                case "DisableOnPressed":
                    _disableOnPressed = child.gameObject;
                    break;
                case "EnableForeverOnPressed":
                    _enableForeverOnPressed = child.gameObject;
                    break;
                case "DisableForeverOnPressed":
                    _disableForeverOnPressed = child.gameObject;
                    break;
            }
        }
        
        //initialise starting positions
        _enableForeverOnPressed.SetActive(false);
        _disableForeverOnPressed.SetActive(true);
        _enableOnPressed.SetActive(false);
        _disableOnPressed.SetActive(true);
    }
    
    void LateUpdate()
    {
        //true if all the buttons in the group is pressed
        bool _allButtonsArePressed = _buttonList.TrueForAll(x => x.Pressed);

        //No changes have been made
        if (_allButtonsArePressed == _lastState)
            return;
        
        //all buttons are pressed
        if (_allButtonsArePressed)
        {
            //if it is the first time all buttons have been pressed
            if (_firstPress)
            {
                _enableForeverOnPressed.SetActive(true);
                _disableForeverOnPressed.SetActive(false);
                _firstPress = false;
            }
            _enableOnPressed.SetActive(true);
            _disableOnPressed.SetActive(false);
        }
        //Not all buttons are pressed
        else
        {
            _enableOnPressed.SetActive(false);
            _disableOnPressed.SetActive(true);
        }

        //Update laststate
        _lastState = _allButtonsArePressed;
    }
}
