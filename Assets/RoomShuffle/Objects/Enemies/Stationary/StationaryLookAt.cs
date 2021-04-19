using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lets a sprite change its appearance based on where the player is in relation to it
/// </summary>
[RequireComponent(typeof(SpotPlayer))]
[RequireComponent(typeof(SpriteRenderer))]
public class StationaryLookAt : MonoBehaviour
{
    [Tooltip("The amount of time the enemy will wait when looking around before choosing a new direction")]
    public RandomValueBetween LookAroundInterval = (0.5f, 1.5f);

    [Header("Sprites")]
    public Sprite LookUpLeftSprite;
    public Sprite LookDownLeftSprite;
    public Sprite LookUpRightSprite;
    public Sprite LookDownRightSprite;

    private IEnumerator Start()
    {
        var spotter = GetComponent<SpotPlayer>();
        var renderer = GetComponent<SpriteRenderer>();
        var lastLookAroundIndex = -3; //-3 is just a number that won't naturally be selected by the random call

        while (true)
        {
            int rotationIndex;

            if (spotter.InPursuit)
            {
                //This is just so that if the enemy sees the player eye-to-eye, they shouldn't look up
                Vector2 offsetToTarget = Vector2.down * 0.5f;

                //Get the player's direction
                var angle = Vector2.SignedAngle(spotter.BlindChaseDirection + offsetToTarget, Vector2.up);
                rotationIndex = (int)Mathf.Floor(angle / 90);

                yield return new WaitForEndOfFrame();

            }
            else
            {
                do
                {
                    rotationIndex = Random.Range(-2, 2);
                } while (lastLookAroundIndex == rotationIndex);
                lastLookAroundIndex = rotationIndex;
                
                yield return new WaitForSeconds(Commons.GetEffectValue(LookAroundInterval.Pick(), EffectValueType.EnemyWaitTime));
            }

            //Update sprite to look at the player
            renderer.sprite = rotationIndex switch
            {
                -2 => LookDownLeftSprite,
                -1 => LookUpLeftSprite,
                0 => LookUpRightSprite,
                1 => LookDownRightSprite,
                _ => renderer.sprite,
            };
        }  
        

    }
}
