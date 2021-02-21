using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedMovementRadius : MonoBehaviour
{
    

    public float BoundaryRadius;
    public bool InHomeRadius { get; private set; }
    [NonSerialized] 
    public Vector2 Home;
    private Flippable _flippable;
    private bool _overrideBoundary;
    private void Start()
    {
        Home = transform.position;
        _flippable = GetComponent<Flippable>();
        _overrideBoundary = false;
    }

    private void Update()
    {
        if (Vector2.Distance(Home, transform.position) >= BoundaryRadius && !_overrideBoundary)
        {
            InHomeRadius = false;
            
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
    
    public void SetOverrideBoundary(bool b)
    {
        _overrideBoundary = b;
    }
    
}
