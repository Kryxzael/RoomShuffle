using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyTypeUI
{
    Puzzle,
    General
}

public class KeySlot : ItemUIManager
{
    public Sprite _keySprite;
    public KeyTypeUI KeyType;
    
    private Inventory _inventory;
    private int _lastNumberOfKeys;
    private int _currentNumberOfKeys;
    private TextSmack _textSmack;
    
    void Start()
    {
        _inventory = Commons.Inventory;
        _textSmack = _text.GetComponent<TextSmack>();
        SetItemImageFill(0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (KeyType)
        {
            case KeyTypeUI.General:
                _currentNumberOfKeys = _inventory.GeneralKeys;
                break;
            case KeyTypeUI.Puzzle:
                _currentNumberOfKeys = _inventory.PuzzleKeys;
                break;
            default: 
                throw new InvalidOperationException();
        }

        //if the number of keys hasn't changed
        if (_lastNumberOfKeys == _currentNumberOfKeys)
            return;

        _lastNumberOfKeys = _currentNumberOfKeys;
        
        //if the player has any keys
        if (_currentNumberOfKeys > 0)
        {
            _text.text = _currentNumberOfKeys.ToString();
            _textSmack.Smack();
            _image.sprite = _keySprite;
            _imageCopy.sprite = _keySprite;
        }
        else
        {
            _text.text = "";
            _image.sprite = null;
            _imageCopy.sprite = null;
        }
    }
}