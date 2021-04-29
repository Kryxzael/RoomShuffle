using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

/// <summary>
/// Shows statistics about a weapon
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class WeaponStats : MonoBehaviour
{
    private TMP_Text _label;
    private PickupBase _pickup;

    public WeaponInstance DisplayedWeapon;

    private string _unformatedText;

    void Start()
    {
        _label = GetComponent<TMP_Text>();
        _unformatedText = _label.text;

        _pickup = GetComponentInParent<PickupBase>();

        if (_pickup)
        {
            var weaponPickup = _pickup.GetComponentInChildren<WeaponPickupBase>();

            if (weaponPickup)
                DisplayedWeapon = weaponPickup.GetWeapon();
        }
            
    }

    private void Update()
    {
        var weapon = DisplayedWeapon;

        if (weapon == null)
        {
            weapon = Commons.Inventory.SelectedWeapon;

            if (weapon == null)
                weapon = Commons.Inventory.FallbackWeaponInstance;
        }
        else if (!_pickup.InPickupRange)
        {
            _label.text = "";
            return;
        }

        _label.text = string.Format(_unformatedText, new object[]
        {
            weapon.Template.Cooldown + " sec",
            weapon.Range.ToString("0.0") + " m",
            weapon.Template.DisplayedProjectileCount,
            weapon.GetDisplayedDamage(),
            weapon.MaxDurability,
        });
    }
}
