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
            
            //
        }
    }

    private IEnumerator CoGenerateAndMoveScreenPlayer()
    {
        SpriteRenderer currentPlayerRenderer = this.GetPlayer().GetComponent<SpriteRenderer>();

        /*
         * Set sprite and size of screen-space player
         */

        Image screenSpacePlayer = Commons.ScreenSpacePlayer;
        screenSpacePlayer.sprite = currentPlayerRenderer.sprite;

        var screenBoundsMin = Camera.main.WorldToScreenPoint(currentPlayerRenderer.bounds.min);
        var screenBoundsMax = Camera.main.WorldToScreenPoint(currentPlayerRenderer.bounds.max);
        var imageFlipped = currentPlayerRenderer.flipX ^ FlipCamera.IsFlipped;

        (screenSpacePlayer.transform as RectTransform).sizeDelta = new Vector2(Mathf.Abs(screenBoundsMax.x - screenBoundsMin.x), Mathf.Abs(screenBoundsMax.y - screenBoundsMin.y));

        if (imageFlipped)
            screenSpacePlayer.transform.localScale *= -1;

        /*
         * Load next level and move player
         */
        const float MOVE_SPEED = 2.5f;


        Vector2 startPosition = Camera.main.WorldToScreenPoint(currentPlayerRenderer.transform.position);

        Commons.RoomGenerator.GenerateNext();
        yield return new WaitForEndOfFrame();

        Vector2 endPosition = Camera.main.WorldToScreenPoint(this.GetPlayer().transform.position);

        for (float i = 0; i < 1f; i += MOVE_SPEED * Time.deltaTime)
        {
            screenSpacePlayer.transform.position = Vector3.Lerp(
                a: startPosition,
                b: endPosition,
                t: i
            );

            yield return new WaitForEndOfFrame();
        }

        screenSpacePlayer.transform.position = new Vector3(99999f, 99999f);

        if (imageFlipped)
            screenSpacePlayer.transform.localScale *= -1;
    }
}
