using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [Tooltip("The platform(s) that circles around")]
    public GameObject Platform;

    [Tooltip("Number of platforms to be spawned")]
    public int NumberOfPlatforms;
    
    [Tooltip("Number of blank space that should take place. (absence of platforms)")]
    public int NumberOfBlankSpace;
    
    [Tooltip("The speed of the ferris wheel object")] 
    public float Speed;

    [Tooltip("The radius of the ferris wheel")] 
    public float Radius;

    [Tooltip("How fast the circle expands and shrinks")]
    public float BreathSpeed;
    
    [Tooltip("How much the circle shrinks and expands")]
    public float BreathAmplitude;

    //List of all circularMotions on the ferris wheel
    private List<CircularMotion> cmList = new List<CircularMotion>();
    
    //The time used to keep track of the "breathing"
    private float _time;

    void Start()
    {

        for (int i = 0; i < NumberOfPlatforms; i++)
        {
            //instantiate the object and get circular motion
            GameObject obj = Instantiate(Platform, transform.Position2D(), Quaternion.identity, transform);
            CircularMotion circularMotion = obj.GetComponent<CircularMotion>();

            if (circularMotion == null)
                circularMotion = obj.AddComponent<CircularMotion>();

            //Add object to list
            cmList.Add(circularMotion);

            //sets the angle
            circularMotion.SetAngle(360 / (NumberOfPlatforms + NumberOfBlankSpace) * i);
            
            //Set speed and radius for each ferris wheel object
            circularMotion.Radius = Radius;
            circularMotion.Speed = Speed;
            
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
}
