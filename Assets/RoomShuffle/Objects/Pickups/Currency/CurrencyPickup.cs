using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.RoomShuffle.Objects.Pickups.Currency
{ 
    /// <summary>
    /// A pickup that gives the player currency
    /// </summary>
    public class CurrencyPickup : PickupScript
    {
        [Tooltip("The worth of the pickup in currency")]
        public int Value;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override void OnPickup()
        {
            if (Commons.CurrentRoomEffects.HasFlag(RoomEffects.ValuePickups))
                Commons.Inventory.Currency += (int)Math.Round(Value * Commons.RoomEffectController.ValuePickupsMultiplier);
            else
                Commons.Inventory.Currency += Value;
        }
    }
}
