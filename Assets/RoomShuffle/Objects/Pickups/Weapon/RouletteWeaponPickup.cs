using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class RouletteWeaponPickup : WeaponPickupBase
{
    private WeaponInstance _weapon;
    private SpriteRenderer _renderer;

    protected override void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        ChangeWeapon();
        InvokeRepeating(nameof(ChangeWeapon), 0.25f, 0.25f);
        base.Start();
    }

    public void ChangeWeapon()
    {
        _weapon = Commons.RoomGenerator.RoomParameterBuilder.WeaponTemplates[UnityEngine.Random.Range(0, Commons.RoomGenerator.RoomParameterBuilder.WeaponTemplates.Count)]
            .CreateWeaponInstance();

        _renderer.sprite = GetWeapon().Template.Icon;
    }

    public override WeaponInstance GetWeapon()
    {
        return _weapon;
    }
}
