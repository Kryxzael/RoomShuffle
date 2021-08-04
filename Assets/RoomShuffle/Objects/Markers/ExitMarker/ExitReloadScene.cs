using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Triggers the iris-out animation. And then reloads the primary scene
/// </summary>
public class ExitReloadScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
        {
            FadeAnimation fade = FindObjectOfType<FadeAnimation>();
            fade.BeginFade(OnBetweenFades, () => TransitionController.CoGenerateAndMoveScreenPlayerPre(fade));
        }
    }

    private IEnumerator OnBetweenFades()
    {
        SceneManager.LoadScene("RoomGenerationTest");
        Time.timeScale = 1f;
        yield break;
    }
}
