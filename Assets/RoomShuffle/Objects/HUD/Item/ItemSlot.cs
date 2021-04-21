using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

public class ItemSlot : ItemUIManager
{
    public Sprite BlankSprite;
    public Sprite AttackUpSprite;
    public Sprite SlowDownSprite;
    public Sprite DefenseUpSprite;

    private void FixedUpdate()
    {
        //Gets the first active power-up (the power-up to display)
        PowerUp? shownPowerUp = typeof(PowerUp).GetEnumValues()
            .Cast<PowerUp?>()
            .FirstOrDefault(i => i.HasValue && Commons.PowerUpManager.HasPowerUp(i.Value));

        Sprite sprite = shownPowerUp switch
        {
            PowerUp.AttackUp => AttackUpSprite,
            PowerUp.SlowDown => SlowDownSprite,
            PowerUp.DefenseUp => DefenseUpSprite,
            _ => BlankSprite,
        };

        Image.sprite = sprite;
        ImageCopy.sprite = sprite;

        if (shownPowerUp is PowerUp pow)
        {
            //Max-ing here to prevent DIV/0
            SetItemImageFill(1f - Commons.PowerUpManager.GetTimeLeft(pow) / Commons.PowerUpManager.GetMaximumTime(pow));
        }
    }
}
