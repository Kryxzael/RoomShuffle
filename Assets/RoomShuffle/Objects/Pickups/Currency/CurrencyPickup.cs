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
    public class CurrencyPickup : Pickup
    {
        [Tooltip("The worth of the pickup in currency")]
        public int Value;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnPickup()
        {
            Commons.Inventory.Currency += Value;
        }
    }
}
