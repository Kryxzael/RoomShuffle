using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        for (int i = 0; i < NumberOfPlatforms; i++)
        {
            //instantiate the object and get circular motion
            GameObject obj = Instantiate(Platform, transform.Position2D(), Quaternion.identity, transform);
            CircularMotion circularMotion = obj.GetComponent<CircularMotion>();

            if (circularMotion == null)
                circularMotion = obj.AddComponent<CircularMotion>();

            //sets the angle
            circularMotion.SetAngle(360 / (NumberOfPlatforms + NumberOfBlankSpace) * i);
            
            //Set speed and radius for each ferris wheel object
            circularMotion.Radius = Radius;
            circularMotion.Speed = Speed;
            
        }
    }
}
