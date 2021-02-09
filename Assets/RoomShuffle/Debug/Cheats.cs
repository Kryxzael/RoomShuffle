using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class Cheats : MonoBehaviour
{
    [Tooltip("Whether the player can always jump, regardless of their grounded state")]
    public bool MoonJump;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            transform.position = default;

        if (Input.GetKeyDown(KeyCode.F2))
            MoonJump = !MoonJump;

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (Time.timeScale < 1f)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0.5f;
        }
    }
}
