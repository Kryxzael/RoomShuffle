using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

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

    public IEnumerator OnPlayerDied()
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
}
