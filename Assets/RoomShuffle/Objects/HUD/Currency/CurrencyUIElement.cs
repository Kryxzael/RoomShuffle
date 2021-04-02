using System;
using System.Collections;
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
        private int _lastKnownCurrencyValue = 0;
        private bool _counterRunning;

        /* *** */

        private TextMeshProUGUI _label;

        private TextSmack _smack;

        private void Awake()
        {
            _label = GetComponent<TextMeshProUGUI>();
            _smack = GetComponent<TextSmack>();
        }

        private void Update()
        {
            if (Commons.Inventory.Currency != _lastKnownCurrencyValue)
            {
                int currentCount = _lastKnownCurrencyValue;
                _lastKnownCurrencyValue = Commons.Inventory.Currency;

                if (!_counterRunning)
                {
                    _counterRunning = true;
                    StartCoroutine(CoCount(currentCount));
                }


            }
        }

        /// <summary>
        /// Counts up or down the currency counter
        /// </summary>
        /// <param name="startValue"></param>
        /// <returns></returns>
        private IEnumerator CoCount(int startValue)
        {
            int count = startValue;

            while (count != _lastKnownCurrencyValue)
            {
                if (count > _lastKnownCurrencyValue)
                {
                    count--;
                }
                else
                {
                    count++;
                }

                _label.text = count.ToString();

                if (_smack != null)
                    _smack.Smack();

                yield return new WaitForSecondsRealtime(0.1f);
            }

            _counterRunning = false;
        }
    }
}
