using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Lets the player jump of a wall
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WallJump : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Collider2D _collider;

    [Tooltip("The force that will be applied by a wall jump")]
    public float WallJumpForce = 10f;

    [Tooltip("The maximum speed the player is allowed to fall when adjacent to a wall and ready to wall jump")]
    public float MaxFallSpeedOnWall = 5f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //The player cannot wall jump at a repelling wall or whilst underwater
        if (RepellingWall.PlayerIsInRepellingArea || Water.IsSubmerged(_rigid))
            return;

        ///Gets the normal of the adjacent wall (if there is one)
        Vector2? wallNormal = GetWallNormal();

        //If there is an adjacent wall
        if (wallNormal != null)
        {
            DebugScreenDrawer.Enable("wallang", "wallang: " + Vector2.Angle(wallNormal.Value, Vector2.up));

            //If the player attempts to jump while next to a wall, create a wall jump
            if (Input.GetButtonDown("Jump") && !this.OnGround2D())
            {
                _rigid.velocity = _rigid.velocity.SetY(0f);
                _rigid.AddForce((wallNormal.Value + Vector2.up * 0.75f).normalized * WallJumpForce, ForceMode2D.Impulse);
            }

            //Restrict maximum falling speed
            _rigid.velocity = _rigid.velocity.SetY(Mathf.Max(_rigid.velocity.y, -MaxFallSpeedOnWall));        
        }
        else
        {
            DebugScreenDrawer.Enable("wallang", "wallang: N/A");
        }
    }

    /// <summary>
    /// Gets the normal for an adjacent wall
    /// </summary>
    /// <returns></returns>
    public Vector2? GetWallNormal()
    {
        //How far to check
        const float CHECK_DISTANCE = 0.25f;

        RaycastHit2D hit;

        //Checks for right wall
        if (hit = Physics2D.Raycast(transform.position, Vector2.right, _collider.bounds.size.x / 2f + CHECK_DISTANCE, Commons.Masks.GroundOnly))
            return hit.normal;

        //Checks for left wall
        if (hit = Physics2D.Raycast(transform.position, Vector2.left, _collider.bounds.size.x / 2f + CHECK_DISTANCE, Commons.Masks.GroundOnly))
            return hit.normal;

        //No wall found
        return null;
    }
}
