using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the exit point of the room
/// </summary>
public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The player has reached the room's exit
        if (collision.gameObject.IsPlayer())
        {
            //Fade out and generate a new room
            FindObjectOfType<FadeAnimation>().BeginFade(CoGenerateAndMoveScreenPlayer);
        } 
    }

    private IEnumerator CoGenerateAndMoveScreenPlayer()
    {
        //The world-space player
        SpriteRenderer worldSpacePlayerRenderer = this.GetPlayer().GetComponent<SpriteRenderer>();

        //The player that is rendered on the canvas
        Image screenSpacePlayer = Commons.ScreenSpacePlayer;

        /*
         * Set sprite and size of screen-space player
         */

        //Give the screen-space player the same sprite as the current frame of the actual player's animation
        screenSpacePlayer.sprite = worldSpacePlayerRenderer.sprite;

        //Calculate the bounds of the player in screen-space coordinates
        var screenBoundsMin = Camera.main.WorldToScreenPoint(worldSpacePlayerRenderer.bounds.min);
        var screenBoundsMax = Camera.main.WorldToScreenPoint(worldSpacePlayerRenderer.bounds.max);

        //Apply the screen-size of the actual player to the screen space player
        screenSpacePlayer.rectTransform.sizeDelta = new Vector2(Mathf.Abs(screenBoundsMax.x - screenBoundsMin.x), Mathf.Abs(screenBoundsMax.y - screenBoundsMin.y));

        //Flip the player if need be
        var imageFlipped = worldSpacePlayerRenderer.flipX ^ FlipCamera.IsFlipped;

        if (imageFlipped)
            screenSpacePlayer.transform.localScale = screenSpacePlayer.transform.localScale.SetX(-screenSpacePlayer.transform.localScale.x);

        /*
         * Load next level and slide the player
         */

        //Controls the speed of the sliding animation
        const float MOVE_SPEED = 2.5f;

        //The position the animation will start at (old player position)
        Vector2 startPosition = Camera.main.WorldToScreenPoint(worldSpacePlayerRenderer.transform.position);

        //Load next level
        Commons.RoomGenerator.GenerateNext();
        yield return new WaitForEndOfFrame();

        //The position the animation will end at (new player position)
        Vector2 endPosition = Camera.main.WorldToScreenPoint(this.GetPlayer().transform.position);

        //Execute animation
        for (float i = 0; i < 1f; i += MOVE_SPEED * Time.deltaTime)
        {
            screenSpacePlayer.transform.position = Vector3.Lerp(
                a: startPosition,
                b: endPosition,
                t: i
            );

            yield return new WaitForEndOfFrame();
        }

        /*
         * Reset
         */

        //Hide the screen-space player
        screenSpacePlayer.transform.position = new Vector3(99999f, 99999f);

        //If the screen-space player is flipped, flip them back to reset
        if (imageFlipped)
            screenSpacePlayer.transform.localScale = screenSpacePlayer.transform.localScale.SetX(-screenSpacePlayer.transform.localScale.x);
    }
}
