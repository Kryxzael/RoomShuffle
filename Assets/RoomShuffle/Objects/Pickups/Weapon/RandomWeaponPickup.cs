using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon pickup that is chosen randomly based on the current room
/// </summary>
public class RandomWeaponPickup : WeaponPickupBase
{
    private WeaponInstance _instance;

    protected override void Start()
    {
        if (Commons.RoomGenerator.CurrentRoomConfig == null)
            throw new InvalidOperationException("RandomWeaponPickup cannot be used outside a generated room");

        if (!Commons.RoomGenerator.CurrentRoomConfig.WeaponEnumerator.MoveNext())
        {
            Debug.LogWarning("WeaponEnumerator ran out of weapons to generate");
            return;
        }

        _instance = Commons.RoomGenerator.CurrentRoomConfig.WeaponEnumerator.Current.CreateWeaponInstance();

        base.Start();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override WeaponInstance GetWeapon()
    {
        return _instance;
    }
}