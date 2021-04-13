using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [Tooltip("The speed of the ferris wheel object")]
    public float Speed;

    [Tooltip("The radius of the ferris wheel")]
    public float Radius;

    [Header("Platform")]
    [Tooltip("The platform(s) that circles around")]
    public GameObject Platform;

    [Tooltip("Number of platforms to be spawned")]
    public int NumberOfPlatforms;

    [Header("Tiling Width")]
    [Tooltip("If enabled, the platforms' tiling width will be overridden by the ferris wheel")]
    public bool SetTilingWidth = true;

    [Tooltip("The tiling width of the objects to spawn")]
    public int PlatformWidth = 4;
    
    [Header("Blank Slots")]
    [Tooltip("Number of blank space that should take place. (absence of platforms)")]
    public int NumberOfBlankSpace;

    [Header("Breathing Effect")]
    [Tooltip("How fast the circle expands and shrinks")]
    public float BreathSpeed;
    
    [Tooltip("How much the circle shrinks and expands")]
    public float BreathAmplitude;
    
    [Header("Visuals")]
    [Tooltip("The 'arms' that points to the pivot point")]
    public GameObject Arm;

    //List of all circularMotions on the ferris wheel
    private List<CircularMotion> cmList = new List<CircularMotion>();
    
    //The time used to keep track of the "breathing"
    private float _time;

    void Start()
    {

        for (int i = 0; i < NumberOfPlatforms; i++)
        {
            //instantiate the object and add circular motion
            GameObject obj = Instantiate(Platform, transform.Position2D(), Quaternion.identity, transform);
            CircularMotion circularMotion = obj.GetComponent<CircularMotion>();

            if (circularMotion == null)
                circularMotion = obj.AddComponent<CircularMotion>();

            //Resize sprite
            if (SetTilingWidth && obj.GetComponent<SpriteRenderer>() is SpriteRenderer spr && spr)
                spr.size = spr.size.SetX(PlatformWidth);


            //Add object to list
            cmList.Add(circularMotion);

            //sets the angle
            circularMotion.SetAngle(360 / (NumberOfPlatforms + NumberOfBlankSpace) * i);
            
            //Set speed and radius for each ferris wheel object
            circularMotion.Radius = Radius;
            circularMotion.Speed = Speed;

            //Set visual arm for circular motion
            if (Arm)
            {
                circularMotion.Arm = Arm;
            }

        }
    }

    private void Update()
    {
        _time += Time.deltaTime;
        
        //foreach circularmotion on the ferris wheel
        foreach (CircularMotion cm in cmList)
        {
            //Edit the radius of the circularmotion to expand or shrink (from 1 * BreathAmplitude to -1 * Breathamplitude)
            cm.Radius = Radius + (float)Math.Sin(_time * BreathSpeed) * BreathAmplitude;
        }
        
        //Keeps the _time variable low
        if (_time > (float)Math.PI * 2)
        {
            _time -= (float)Math.PI * 2;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
