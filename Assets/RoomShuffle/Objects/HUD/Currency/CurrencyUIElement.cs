using System.Collections;

using TMPro;

using UnityEngine;

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

    private MultiSoundPlayer _multiSoundPlayer;

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
        _smack = GetComponent<TextSmack>();
        _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
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
        bool lastTickPlayedSound = false;

        while (count != _lastKnownCurrencyValue)
        {
            if (count > _lastKnownCurrencyValue)
            {
                count--;

                if (lastTickPlayedSound = !lastTickPlayedSound)
                    _multiSoundPlayer.PlaySound(0);
            }
            else
            {
                count++;

                if (lastTickPlayedSound = !lastTickPlayedSound)
                    _multiSoundPlayer.PlaySound(1);
            }

            SetLabel(count.ToString());

            if (_smack != null)
                _smack.Smack();

            yield return new WaitForSecondsRealtime(0.05f);
        }

        _counterRunning = false;
    }

    /// <summary>
    /// Sets the label for currency and adds the desired sprite
    /// </summary>
    /// <param name="text"></param>
    private void SetLabel(string text)
    {
        const int SPRITE_INDEX = 0;
        _label.text = text + $"<sprite={SPRITE_INDEX}>";
    }
}