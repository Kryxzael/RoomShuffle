using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

using UnityEngine;

/// <summary>
/// Groups contact switches into an AND gate
/// </summary>
public class ContactBlockGroup : MonoBehaviour
{
    //The blocks in the contact switch group
    private List<ContactSwitch> _blocks = new List<ContactSwitch>();

    //The prize to spawn when all contact switches are active
    private GameObject _prize;

    //How many blocks were unactivated last frame
    private int _lastNumberOfUnactivatedBlocks;

    /* *** */

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
        
        //Disable prize
        _prize.SetActive(false);

        //Count the amount of inactive blocks
        _lastNumberOfUnactivatedBlocks = _blocks.Count(x => !x.On);
    }
    
    void Update()
    {
        //get number of blocks that are turned off
        int currentNumberOfOffBlocks = _blocks.Count(x => !x.On);

        //If all blocks are on
        if (currentNumberOfOffBlocks == 0 && _lastNumberOfUnactivatedBlocks > 0)
        {
            //Play "tada" sound
            _multiSoundPlayer.PlaySound(1);
            _prize.SetActive(true);
            _blocks.ForEach(x => x.Lock());
        }
        
        //positive: The number of on-blocks have gone up
        else if (currentNumberOfOffBlocks < _lastNumberOfUnactivatedBlocks)
        {
            //play spound with increased pitch
            _multiSoundPlayer.PlaySound(0, 1 + ((_blocks.Count - currentNumberOfOffBlocks) / (float)_blocks.Count));
        }

        //Negative: The number of on- blocks have gone down
        else if (currentNumberOfOffBlocks > _lastNumberOfUnactivatedBlocks)
        {
            //play sound with decreased pitch
            _multiSoundPlayer.PlaySound(0, 1 + ((_blocks.Count - currentNumberOfOffBlocks) / (float)_blocks.Count));
        }

        //update last state
        _lastNumberOfUnactivatedBlocks = currentNumberOfOffBlocks;
        

    }
}
