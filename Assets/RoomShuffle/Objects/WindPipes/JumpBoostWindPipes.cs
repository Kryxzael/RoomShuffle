using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostWindPipes : MonoBehaviour
{
    private AreaEffector2D strongWind;
    private AreaEffector2D middleWind;
    private AreaEffector2D weakWind;

    private float strongForce;
    private float middleForce;
    private float weakForce;

    [Tooltip("How much the force gets multiplied when holding jump")]
    public float ForceMulitplier;
    
    private void Start()
    {
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

        strongForce = strongWind.forceMagnitude;
        middleForce = middleWind.forceMagnitude;
        weakForce = weakWind.forceMagnitude;
        
    }

    void Update()
    {
        float mulitply = 1;
        
        if (Input.GetButton("Jump"))
        {
            mulitply = ForceMulitplier;
        }
        
        Debug.Log(strongWind);

        strongWind.forceMagnitude = strongForce * mulitply;
        middleWind.forceMagnitude = middleForce * mulitply;
        weakWind.forceMagnitude = weakForce * mulitply;
    }
}
