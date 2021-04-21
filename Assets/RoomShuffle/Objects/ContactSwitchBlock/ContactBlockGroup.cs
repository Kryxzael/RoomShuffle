using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using RoomShuffle.Defaults;
using UnityEngine;

public class ContactBlockGroup : MonoBehaviour
{
    private List<ContactSwitch> _blocks = new List<ContactSwitch>();
    private GameObject _prize;

    private bool prizeSpawned = false;

    private int _lastNumberOfOffBlocks;

    private MultiSoundPlayer _multiSoundPlayer;
    void Start()
    {
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
        
        //Add all contactswitch blocks to list
        foreach (Transform child in transform)
        {
            if (child.name == "Blocks")
            {
                foreach (Transform childchild in child)
                {
                    _blocks.Add(childchild.GetComponent<ContactSwitch>());
                }
            }
            
            if (child.name == "Prize")
            {
                _prize = child.gameObject;
            }
        }
        
        _prize.SetActive(false);

        _lastNumberOfOffBlocks = _blocks.Count(x => x.On == false);
    }

    // Update is called once per frame
    void Update()
    {
        int currentNumberOfOffBlocks = _blocks.Count(x => x.On == false);

        if (currentNumberOfOffBlocks == 0 && _lastNumberOfOffBlocks > 0)
        {
            _multiSoundPlayer.PlaySound(1);
            _prize.SetActive(true);
            _blocks.ForEach(x => x.Lock());
        }
        
        //positive
        else if (currentNumberOfOffBlocks < _lastNumberOfOffBlocks)
        {
            _multiSoundPlayer.PlaySound(0, 1 + ((_blocks.Count - currentNumberOfOffBlocks) / (float)_blocks.Count));
        }
        //Negative
        else if (currentNumberOfOffBlocks > _lastNumberOfOffBlocks)
        {
            _multiSoundPlayer.PlaySound(0, 1 + ((_blocks.Count - currentNumberOfOffBlocks) / (float)_blocks.Count));
        }

        _lastNumberOfOffBlocks = currentNumberOfOffBlocks;
        

    }
}
