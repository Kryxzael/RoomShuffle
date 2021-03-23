using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// State machine that keeps track of the spawn point ("Home") of the enemy and the near area.
/// </summary>

[RequireComponent(typeof(Flippable))]
public class LimitedMovementRadius : MonoBehaviour
{
    [Tooltip("The radius from the spawn point the enemy will be able to roam freely in")]
    public float BoundaryRadius;
    
    //True if the enemy is in its home radius
    public bool InHomeRadius { get; private set; }

    //The spawn point of the enemy. May be updated
    [NonSerialized] 
    public Vector2 Home;
    
    //If true: InHomeRadius is always true
    public bool OverrideBoundary { get; set; }
    
    private Flippable _flippable;
    
    private void Start()
    {
        //sets the initial home position
        Home = transform.position;
        
        _flippable = GetComponent<Flippable>();
    }

    private void Update()
    {
        //If the Boundry radius is 0 or Override Boundries is true: InhomeRadius is always true
        if (BoundaryRadius == 0 || OverrideBoundary){
            InHomeRadius = true;
            return;
        }

        //if the distance from the enemy to home position is larger than the chosen radius
        if (Vector2.Distance(Home, transform.position) >= BoundaryRadius)
        {
            //Out of radius
            InHomeRadius = false;
            
            //flip the flippable to face home
            if (Home.x <= transform.position.x)
            {
                _flippable.Direction = Direction1D.Left;
            }
            else
            {
                _flippable.Direction = Direction1D.Right;
            }
        }
        else
        {
            InHomeRadius = true;
        }
    }

}
