using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Drops an item that the player is in need of right now
/// </summary>
[CreateAssetMenu(menuName = "Loot Tables/Required Item Loot Table")]
public class RequiredItemLootTable : LootTable
{
    public GameObject HealthPickup;
    public GameObject MaxHealthPickup;
    public GameObject WeaponPickup;
    public GameObject WeaponRepairPickup;
    public GameObject LargeCurrency;
    public GameObject SmallCurrency;

    public override IEnumerable<GameObject> GetLootItems()
    {
        //Player is low on health, drop health item
        if (InNeedOfHealth())
        {
            if (UnityEngine.Random.value > 0.8f)
                yield return MaxHealthPickup;

            else
                yield return HealthPickup;
        }

        //Player is low on ammo, drop something a weapon
        else if (InNeedOfWeapons())
        {
            if (UnityEngine.Random.value >= 0.75f)
                yield return WeaponRepairPickup;

            else
                yield return WeaponPickup;
        }

        //Player is poor
        else if (InNeedOfMoney())
        {
            yield return LargeCurrency;
        }
        else
        {
            yield return SmallCurrency;
        }
    }

    /// <summary>
    /// Is the player's health low?
    /// </summary>
    /// <returns></returns>
    public static bool InNeedOfHealth()
    {
        return Commons.PlayerHealth.Health <= Commons.PlayerHealth.MaximumHealth / 3f;
    }

    /// <summary>
    /// Is the player's ammo low?
    /// </summary>
    /// <returns></returns>
    public static bool InNeedOfWeapons()
    {
        bool[] ammoIsLow = new bool[Commons.Inventory.WeaponSlots.Length];

        for (int i = 0; i < ammoIsLow.Length; i++)
        {
            var weapon = Commons.Inventory.WeaponSlots[i];

            if (weapon == null)
            {
                ammoIsLow[i] = true;
            }
            else
            {
                ammoIsLow[i] = weapon.Durability <= weapon.MaxDurability / 4f;
            }
        }

        return ammoIsLow.All(i => i);
    }

    /// <summary>
    /// Is the player's currency count low?
    /// </summary>
    /// <returns></returns>
    public static bool InNeedOfMoney()
    {
        return Commons.Inventory.Currency < 50;
    }
}