using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ray-casted hurtbox with the ability to be on a cycle
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(HurtBox))]
public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private HurtBox _hurtbox;

    /* *** */


    [Header("Appearance")]
    [Tooltip("The material to use on the laser when it's charging")]
    public Material ChargingMaterial;

    [Tooltip("The material to use on the laser when it's firing")]
    public Material FiringMaterial;

    [Header("Times")]
    [Tooltip("The duration of a cycle. This value can be ignored if Duty Cycle is 0 or 1")]
    public float CycleTime = 2f;

    [Tooltip("The ratio between the laser's on-state and off-state. 0 = always off, 1 = always on, 0.5 = even distribution")]
    [Range(0f, 1f)]
    public float DutyCycle = 0.5f;

    [Tooltip("How offset the laser's cycle is from other lasers")]
    [Range(0f, 1f)]
    public float CycleOffset;

    /* *** */

    /// <summary>
    /// Is the laser currently active
    /// </summary>
    public bool IsOn { get; private set; }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        //The hurtbox of this object isn't used normally (with collision) but is applied by the laser programmatically
        _hurtbox = GetComponent<HurtBox>();
    }

    private void Update()
    {
        /*
         * Figure out if the laser should be on
         */
        //The progress (percentage) of the current cycle with offset taken into account
        var cycleProgress = (Time.time + (CycleOffset * CycleTime)) % CycleTime / CycleTime;

        //The laser is on if the cycle is less than the duty cycle of the laser
        IsOn = cycleProgress < DutyCycle;

        /*
         * Set laser start and end point
         */
        const float MAX_DISTANCE = 200f;

        //Start is at the emitter's position
        var start = transform.position;

        //End is by default MAX_DISTANCE units away from the emitter
        var end = start + transform.up * MAX_DISTANCE;

        //If the laser is obstructed by something, that thing will be the end point
        var raycastHit = Physics2D.Raycast(transform.position, transform.up, MAX_DISTANCE, Commons.Masks.GroundOnly);

        if (raycastHit)
            end = raycastHit.point;

        //Update positions
        _lineRenderer.SetPositions(new[] { start, end });

        /*
         * Sets the visibility of the laser
         */
        _lineRenderer.forceRenderingOff = !(cycleProgress < DutyCycle || cycleProgress > 1f - DutyCycle / 2f);

#if UNITY_EDITOR
        //In the editor, the laser is always on (Time is weird in edit mode) unless the laser is set to always be off
        if (!Application.isPlaying)
        {
            IsOn = DutyCycle != 0f;
            _lineRenderer.forceRenderingOff = !IsOn;
        }

#endif

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
