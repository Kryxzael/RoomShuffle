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
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Buttons"))
            {
                child.Cast<Transform>().ForEach(x => _buttonList.Add(x.GetComponent<Button>()));
                continue;
            }

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
        
        _enableForeverOnPressed.SetActive(false);
        _disableForeverOnPressed.SetActive(true);
        _enableOnPressed.SetActive(false);
        _disableOnPressed.SetActive(true);
    }
    
    void LateUpdate()
    {
        bool AllButtonsArePressed = _buttonList.TrueForAll(x => x.Pressed);

        //No changes have been made
        if (AllButtonsArePressed == _lastState)
            return;
        
        //changes have been made
        if (AllButtonsArePressed)
        {
            if (_firstPress)
            {
                _enableForeverOnPressed.SetActive(true);
                _disableForeverOnPressed.SetActive(false);
                _firstPress = false;
            }
            _enableOnPressed.SetActive(true);
            _disableOnPressed.SetActive(false);
        }
        else
        {
            _enableOnPressed.SetActive(false);
            _disableOnPressed.SetActive(true);
        }

        _lastState = AllButtonsArePressed;
    }
}
