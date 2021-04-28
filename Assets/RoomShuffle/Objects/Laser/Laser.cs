using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private LaserAudio _laserAudio;
    private Camera _mainCamera;
    private bool _isVisible;

    /// <summary>
    /// Is the laser currently active
    /// </summary>
    public bool IsOn { get; private set; }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        //The hurtbox of this object isn't used normally (with collision) but is applied by the laser programmatically
        _hurtbox = GetComponent<HurtBox>();

        _laserAudio = Commons.AudioManager.GetComponent<LaserAudio>();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        /*
         * Figure out if the laser should be on
         */
        //The progress (percentage) of the current cycle with offset taken into account
        var cycleProgress = (Commons.GetEffectValue(Time.time, EffectValueType.EnemySpeed) + (CycleOffset * CycleTime)) % CycleTime / CycleTime;

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
            bool lastQueriesStartInCollidersState = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = true;

            RaycastHit2D[] hits = Physics2D.RaycastAll(start, transform.up, Vector2.Distance(start, end), Commons.Masks.HitboxesHurtboxes);

            Physics2D.queriesStartInColliders = lastQueriesStartInCollidersState;

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

            _isVisible = IsLaserOnScreen(start, end) || _isVisible;

        }

        //Laser is not firing
        else
        {
            //Set charge-up material
            _lineRenderer.material = ChargingMaterial;
        }
        
        //add laser to active laser list (to play audio)
        if (_isVisible && IsOn)
        {
            _laserAudio.AddLaser(gameObject);
        }
        else
        {
            _laserAudio.RemoveLaser(gameObject);
        }
    }

    /// <summary>
    /// Checks if the laser is intersecting with the camera.
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <returns></returns>
    private bool IsLaserOnScreen(Vector2 startPosition, Vector2 endPosition)
    {
        float height = 2f * _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;
        Bounds cameraBounds = new Bounds(_mainCamera.transform.position, new Vector3(width, height, 0));
        
        Ray ray = new Ray(startPosition, endPosition - startPosition);
            
        bool intersecting = cameraBounds.IntersectRay(ray, out float length);

        return (length <= Vector2.Distance(startPosition, endPosition) && intersecting);

    }

    private void OnBecameVisible()
    {
        _isVisible = true;
    }

    private void OnBecameInvisible()
    {
        _isVisible = false;
    }

    private void OnDestroy()
    {
        _laserAudio.RemoveLaser(gameObject);
    }
}
