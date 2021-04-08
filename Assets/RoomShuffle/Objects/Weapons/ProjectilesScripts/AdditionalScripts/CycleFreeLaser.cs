using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ray-casted hurtbox with the ability to be on a cycle
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HurtBox))]
public class CycleFreeLaser : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private HurtBox _hurtbox;

    /* *** */


    [Header("Appearance")]
    [Tooltip("The material to use on the laser when it's charging")]
    public Material ChargingMaterial;

    [Tooltip("The material to use on the laser when it's firing")]
    public Material FiringMaterial;
    
    [Tooltip("For how long the laser will charge")]
    public float ChargeTime = 2f;

    [Tooltip("The maximum distance of the laser")]
    public float MaxDistance = 200;

    /* *** */

    /// <summary>
    /// Is the laser currently active
    /// </summary>
    public bool IsOn { get; private set; }

    private float _time;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        //The hurtbox of this object isn't used normally (with collision) but is applied by the laser programmatically
        _hurtbox = GetComponent<HurtBox>();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        IsOn = _time >= ChargeTime;

        /*
         * Set laser start and end point
         */

        //Start is at the emitter's position
        var start = transform.position;

        //End is by default MAX_DISTANCE units away from the emitter
        var end = start + transform.up * MaxDistance;

        //If the laser is obstructed by something, that thing will be the end point
        var raycastHit = Physics2D.Raycast(transform.position, transform.up, MaxDistance, Commons.Masks.GroundOnly);

        if (raycastHit)
            end = raycastHit.point;

        //Update positions
        _lineRenderer.SetPositions(new[] { start, end });

        /*
         * Sets the visibility of the laser
         */
        _lineRenderer.forceRenderingOff = false;

        //Laser is firing
        if (IsOn)
        {
            /*
             * Apply hurtbox
             */
            RaycastHit2D[] hits = Physics2D.RaycastAll(start, transform.up, Vector2.Distance(start, end), Commons.Masks.HitboxesHurtboxes);

            //For each hitbox that was hit, deal damage with the hurtbox
            foreach (var i in hits)
            {
                var hitbox = i.collider.GetComponent<Hitbox>();

                if (hitbox == null)
                    continue;

                hitbox.TryDealDamageBy(_hurtbox);
            }

            //Set firing material
            _lineRenderer.material = FiringMaterial;
        }

        //Laser is not firing
        else
        {
            //Set charge-up material
            _lineRenderer.material = ChargingMaterial;
        }
    }
}