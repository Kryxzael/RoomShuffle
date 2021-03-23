using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

namespace Assets.RoomShuffle.Objects.HUD.Currency
{
    /// <summary>
    /// Applies the player's currency value to a text block
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CurrencyUIElement : MonoBehaviour
    {
        private int? _lastKnownCurrencyValue = null;

        /* *** */

        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (Commons.Inventory.Currency != _lastKnownCurrencyValue)
            {
                _lastKnownCurrencyValue = Commons.Inventory.Currency;
                _label.text = "$" + _lastKnownCurrencyValue;
            }
        }
    }
}
