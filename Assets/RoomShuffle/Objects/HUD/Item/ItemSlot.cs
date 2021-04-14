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

    private void FixedUpdate()
    {
        //Gets the first active power-up (the power-up to display)
        PowerUp shownPowerUp = typeof(PowerUp).GetEnumValues()
            .Cast<PowerUp>()
            .FirstOrDefault(i => Commons.PowerUpManager.HasPowerUp(i));

        Sprite sprite = shownPowerUp switch
        {
            PowerUp.AttackUp => AttackUpSprite,
            PowerUp.SlowDown => SlowDownSprite,
            _ => BlankSprite,
        };

        Image.sprite = sprite;
        ImageCopy.sprite = sprite;

        //Max-ing here to prevent DIV/0
        SetItemImageFill(1f - Commons.PowerUpManager.GetTimeLeft(shownPowerUp) / Commons.PowerUpManager.GetMaximumTime(shownPowerUp));
    }
}
