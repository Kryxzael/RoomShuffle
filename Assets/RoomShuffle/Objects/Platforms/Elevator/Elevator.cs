using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using NUnit.Framework;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Tooltip("The speed of the elevator cart")]
    public float Speed;

    private IEnumerator Start()
    {

        var manager = GetComponentInParent<ElevatorManager>();
        Transform lastCheckpoint = null;

        if (!manager)
        {
            Debug.Log("There is a heckin elevator cart at " + this.transform.position);
        }

        //Set size of cart
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer)
            spriteRenderer.size = spriteRenderer.size.SetX(manager.CartWidth);

        //Sets the speed of the cart
        Speed = manager.CartSpeed;

        //Repeat the process as long as there are checkpoints to reach
        foreach (var destination in GetCheckpointProgression(manager))
        {
            float travelPercentage = 0f;

            if (lastCheckpoint == null)
                travelPercentage = 1f;

            while (travelPercentage < 1f)
            {
                travelPercentage += (Time.deltaTime * Speed) / Vector2.Distance(lastCheckpoint.position, destination.position);
                transform.position = Vector2.Lerp(lastCheckpoint.position, destination.position, travelPercentage);

                //yield return null runs _before_ LateUpdate()
                //yield return new WaitForEndOfFrame() runs _after_ LateUpdate() (which screws with some other scripts)
                yield return null;
            }

            lastCheckpoint = destination;
        }

        //Reached the end of the track. Destroy self
        Destroy(gameObject);
    }

    /// <summary>
    /// Creates an enumerator that yields the checkpoint progression for the elevator
    /// WARNING: This enumerator may never stop yielding and could end in permanent loops
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Transform> GetCheckpointProgression(ElevatorManager manager)
    {
        switch (manager.EOLOption)
        {
            case EndOfLineOption.Destroy:
                //Get every checkpoint...
                foreach (var i in manager.Checkpoints)
                    yield return i;

                //...Then stop
                yield break;
            case EndOfLineOption.Loop:
                while (true)
                {
                    //Get every checkpoint, then loop
                    foreach (var i in manager.Checkpoints)
                        yield return i;
                }

            case EndOfLineOption.Return:

                //Get every checkpoint....
                foreach (var i in manager.Checkpoints)
                    yield return i;

                while (true)
                {
                    //...Then get the same checkpoints backwards (skipping the one we're already at)...
                    foreach (var i in manager.Checkpoints.Reverse().Skip(1))
                        yield return i;

                    //...Then get the same checkpoints forwards (skipping the one we're already at), and loop
                    foreach (var i in manager.Checkpoints.Skip(1))
                        yield return i;
                }
        }
    }
}
