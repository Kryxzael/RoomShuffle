using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Groups multiple buttons together to form an AND gate
/// </summary>
public class ButtonGroup : MonoBehaviour
{
    //All buttons in the button group
    private List<Button> _buttonList = new List<Button>();

    //This game object will only be active when all buttons in group are pressed
    private GameObject _enableOnPressed;

    //This game object will only be inactive when all buttons in group are pressed
    private GameObject _disableOnPressed;

    //This game object will be activated when all buttons in group are pressed
    private GameObject _enableForeverOnPressed;

    //This game object will be deactivated when all buttons in group are pressed
    private GameObject _disableForeverOnPressed;

    //If true, the buttons have not been pressed yet.
    private bool _firstPress = true;

    //The state of the group last frame
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
        
        //initialize starting positions
        _enableForeverOnPressed.SetActive(false);
        _disableForeverOnPressed.SetActive(true);
        _enableOnPressed.SetActive(false);
        _disableOnPressed.SetActive(true);
    }
    
    void LateUpdate()
    {
        //true if all the buttons in the group is pressed
        bool _allButtonsArePressed = _buttonList.TrueForAll(x => x.Pressed);

        //No changes have been made since last frame
        if (_allButtonsArePressed == _lastState)
            return;
        
        //All buttons are pressed
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
