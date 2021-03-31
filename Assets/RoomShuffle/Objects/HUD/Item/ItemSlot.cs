using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : ItemUIManager
{

    private Inventory _inventory;
    void Start()
    {
        _inventory = Commons.Inventory;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO create a listener for items. 
    }
}
