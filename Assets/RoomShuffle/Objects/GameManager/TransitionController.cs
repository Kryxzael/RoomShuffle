using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    private bool _playerDied = false;

    public TextMeshProUGUI GameOverText;

    private void Update()
    {
        //Player has died
        if (Commons.PlayerHealth.Health == 0 && !_playerDied)
        {
            _playerDied = true;
            StartCoroutine(OnPlayerDied());
        }
    }

    private IEnumerator OnPlayerDied()
    {
        //Detach the camera
        Camera.main.transform.SetParent(FindObjectOfType<GeneratedRoom>().transform);

        //Destroy player object
        Destroy(this.GetPlayer());

        //Wait for dramatic effect
        const float GAME_OVER_WAIT_TIME = 2f;
        yield return new WaitForSecondsRealtime(GAME_OVER_WAIT_TIME);

        //Show game over text
        GameOverText.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.anyKeyDown);

        //TODO: Restart game somehow
        SceneManager.LoadScene("RoomGenerationTest");
    }

    /// <summary>
    /// Fades the screen, generates the level, moves the player and then fades in
    /// </summary>
    public void CreateTransitionToNextRoom()
    {
        FadeAnimation fade = FindObjectOfType<FadeAnimation>();

        fade.BeginFade(() => CoGenerateAndMoveScreenPlayer(fade), () => CoGenerateAndMoveScreenPlayerPre(fade), CoGenerateAndMoveScreenPlayerPost);
    }

    private IEnumerator CoGenerateAndMoveScreenPlayerPre(FadeAnimation fade)
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

        //Set position of the iris-out
        fade.FadeMask.transform.position = Camera.main.WorldToScreenPoint(worldSpacePlayerRenderer.transform.position);

        Time.timeScale = 0f;
        yield break;
    }

    private IEnumerator CoGenerateAndMoveScreenPlayerPost()
    {
        Time.timeScale = 1f;
        yield break;
    }

    private IEnumerator CoGenerateAndMoveScreenPlayer(FadeAnimation fade)
    {
        //The world-space player
        SpriteRenderer worldSpacePlayerRenderer = this.GetPlayer().GetComponent<SpriteRenderer>();

        //The player that is rendered on the canvas
        Image screenSpacePlayer = Commons.ScreenSpacePlayer;

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
        for (float i = 0; i < 1f; i += MOVE_SPEED * Time.unscaledDeltaTime)
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

        //Set origin of the iris-in
        fade.FadeMask.transform.position = screenSpacePlayer.transform.position;

        //Hide the screen-space player
        screenSpacePlayer.transform.position = new Vector3(99999f, 99999f);
    }
}
