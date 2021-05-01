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
    private LaserAudio _laserAudio;

    private bool _isVisible;

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

    private Camera _mainCamera;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        //The hurtbox of this object isn't used normally (with collision) but is applied by the laser programmatically
        _hurtbox = GetComponent<HurtBox>();

        _laserAudio = Commons.AudioManager.GetComponent<LaserAudio>();
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
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
        }

        //Laser is not firing
        else
        {
            //Set charge-up material
            _lineRenderer.material = ChargingMaterial;
        }

        _isVisible = IsLaserOnScreen(start, end);

        if (IsOn && _isVisible)
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
        Bounds cameraBounds = new Bounds(_mainCamera.transform.Position2D(), new Vector3(width, height, 0));
        Ray ray = new Ray(startPosition, endPosition - startPosition);
        bool intersecting = cameraBounds.IntersectRay(ray, out float length);
        
        return (length <= Vector2.Distance(startPosition, endPosition) && intersecting) || Commons.IsVectorOnScreen(startPosition, _mainCamera);

    }

    private void OnDestroy()
    {
        _laserAudio.RemoveLaser(gameObject);
    }
}
