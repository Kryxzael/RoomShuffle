using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Multiplies the magnitude of the effector when holding jump  
/// </summary>
public class JumpBoostWindPipes : MonoBehaviour
{
    //Separated windpipes
    private AreaEffector2D strongWind;
    private AreaEffector2D middleWind;
    private AreaEffector2D weakWind;

    //The original magnitude values of the windpipes
    private float strongForce;
    private float middleForce;
    private float weakForce;

    [Tooltip("How much the force gets multiplied when holding jump")]
    public float ForceMulitplier;
    
    private void Start()
    {
        //Find the areaEffectors in children
        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Strong":
                    strongWind = child.GetComponent<AreaEffector2D>();
                    break;
                case "Middle": 
                    middleWind = child.GetComponent<AreaEffector2D>();
                    break;
                case "Weak": 
                    weakWind = child.GetComponent<AreaEffector2D>();
                    break;
            }
        }

        //Set original values
        strongForce = strongWind.forceMagnitude;
        middleForce = middleWind.forceMagnitude;
        weakForce = weakWind.forceMagnitude;
        
    }

    void Update()
    {
        //Set the multplier to desired multiplier if holding jump. else 1.
        float mulitply = Input.GetButton("Jump") ? ForceMulitplier : 1f;
        
        //Multiply magnitude
        strongWind.forceMagnitude = strongForce * mulitply;
        middleWind.forceMagnitude = middleForce * mulitply;
        weakWind.forceMagnitude = weakForce * mulitply;
    }
}
