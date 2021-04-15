using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class ContactBlockGroup : MonoBehaviour
{
    private List<ContactSwitch> _blocks = new List<ContactSwitch>();
    private GameObject _prize;

    private bool prizeSpawned = false;
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_blocks.TrueForAll(x => x.On))
        {
            _prize.SetActive(true);
            
            _blocks.ForEach(x => x.Lock());
        }
    }
}
