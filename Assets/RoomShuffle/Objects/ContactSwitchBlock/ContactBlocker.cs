using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBlocker : MonoBehaviour
{
    private ContactSwitch _contactSwitch;

    private void Awake()
    {
        //Object always inactive when starting
        gameObject.SetActive(false);
    }

    void Start()
    {
        //find parent
        _contactSwitch = GetComponentInParent<ContactSwitch>();
    }

    //Passes the trigger event to the parent
    private void OnTriggerEnter2D(Collider2D other)
    {
        _contactSwitch.TriggerSwitch(other);
    }
}
