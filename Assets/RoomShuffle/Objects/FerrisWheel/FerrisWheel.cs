using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{

    [Tooltip("The platform(s) that circles around")]
    public CircularMotion Platform;

    [Tooltip("Number of platforms to be spawned")]
    public int NumberOfPlatforms;
    
    [Tooltip("The speed of the ferris wheel object")] 
    public float Speed;

    [Tooltip("The radius of the ferris wheel")] 
    public float Radius;
    void Start()
    {
        for (int i = 0; i < NumberOfPlatforms; i++)
        {
            //instantiate the object and get circulat motion
            CircularMotion cm = Instantiate(Platform, transform.Position2D(), Quaternion.identity, transform).GetComponent<CircularMotion>();
            
            //sets the angle
            cm.SetAngle(360 / NumberOfPlatforms * i);
            
            //Set speed and radius for each ferris wheel object
            cm.Radius = Radius;
            cm.Speed = Speed;
        }
    }
}
