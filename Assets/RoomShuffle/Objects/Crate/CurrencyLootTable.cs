using Assets.RoomShuffle.Objects.Pickups.Currency;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// A loot table that contains a certain value in currency items
/// </summary>
[CreateAssetMenu(menuName = "Loot Tables/Currency Loot Table")]
public class CurrencyLootTable : LootTable
{
    [Tooltip("The amount of currency to drop")]
    public RandomValueBetween Value;

    [Tooltip("The available currency pickups")]
    public List<PickupBase> Currencies = new List<PickupBase>();

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<GameObject> GetLootItems()
    {
        int target = Value.PickInt();
        int sum = 0;

        //Sorts the currencies by their value so that higher denominations are prioritized
        var currenciesByValue = Currencies.OrderByDescending(i => i.GetComponentInChildren<CurrencyPickup>().Value);

        //Keep yielding pickups until we've reached our sum
        while (sum < target)
        {
            //Browse the currency items
            foreach (var i in currenciesByValue)
            {
                CurrencyPickup iCurrency = i.GetComponentInChildren<CurrencyPickup>();

                //If the current currency object's value does not exceed the remaining value to yield, then yield it
                if (sum + iCurrency.Value <= target)
                {
                    sum += iCurrency.Value;
                    yield return i.gameObject;
                    break;
                }
            }
        }
    }
}
