using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A weapon object pickup created from a specific template
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpecificWeaponPickup : WeaponPickupBase
{
    //The created weapon instance (created from the template)
    private WeaponInstance _instance;

    [Tooltip("The weapon that will be created for this pickup")]
    public WeaponTemplate Template;

    protected override void Start()
    {
        if (!Template)
            throw new NullReferenceException("The SpecificWeaponInstance did not have a set WeaponTemplate");

        _instance = Template.CreateWeaponInstance();

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
