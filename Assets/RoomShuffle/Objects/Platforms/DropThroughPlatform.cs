using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(PlatformEffector2D))]

//A script that let's the player drop through platformEffector2D platforms
public class DropThroughPlatform : DetectObjectsOn
{
    private PlatformEffector2D _platformEffector2D;

    //True if the player was chrouching last frame
    private bool _crouchingLastFrame;

    //The original angle of the surfaceArc
    private float originalAngle;

    //the player object
    private GameObject _player;
    void Start()
    {
        _platformEffector2D = GetComponent<PlatformEffector2D>();

        originalAngle = _platformEffector2D.surfaceArc;

        _player = this.GetPlayer();
    }
    
    void Update()
    {
        //If the player isn't standing on the platform: return
        if (!IsPlayerOn())
        {
            _crouchingLastFrame = checkCrouch();
            return;
        }


        if (Input.GetAxisRaw("Vertical") < 0f && _crouchingLastFrame == false)
        {
            StartCoroutine(CoNoSurfaceArc());
        }

        _crouchingLastFrame = checkCrouch();
    }

    private IEnumerator CoNoSurfaceArc()
    {
        _platformEffector2D.surfaceArc = 0;
        yield return new WaitForSeconds(0.3f);
        _platformEffector2D.surfaceArc = originalAngle;
    }

    /// <summary>
    /// Returns true if player is on platform
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerOn()
    {
        return ObjectsOn.Any(x => x == _player.transform);
    }

    /// <summary>
    /// return true if the player is chrouching
    /// </summary>
    /// <returns></returns>
    private bool checkCrouch()
    {
        if (Input.GetAxisRaw("Vertical") < 0f)
        {
            return true;
        }

        return false;
    }
}

public enum FallThroughAction
{
    DownAndJump,
    Down
}
