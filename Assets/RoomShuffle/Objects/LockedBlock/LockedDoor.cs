using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A door that the player can enter if they have a general key
/// </summary>
public class LockedDoor : MonoBehaviour
{
    private bool _playerInRange;

    [Tooltip("The room parameter override object that will generate the secret room")]
    public ParameterBuilderOverride GeneratorOverride;

    private void Update()
    {
        if (_playerInRange && Input.GetButtonDown("Interact"))
        {
            if (Commons.Inventory.GeneralKeys > 0)
            {
                Commons.Inventory.GeneralKeys--;

                Commons.RoomGenerator.RoomParameterBuilderOverrides.Push(Instantiate(GeneratorOverride));
                Commons.RoomGenerator.GenerateNext();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _playerInRange = false;
    }
}
