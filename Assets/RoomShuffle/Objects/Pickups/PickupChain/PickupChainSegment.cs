using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A segment in a pickup chain, which will only spawn when the previous pickup chain is picked up
/// </summary>
public class PickupChainSegment : MonoBehaviour
{
    private bool _applicationInShutdown;

    [Header("Chain")]
    [Tooltip("The group the segment is part of. Each segment in the same chain should have the same ID")]
    public int ChainGroup;

    [Tooltip("The step index in the current chain. When a segment is picked up it will look for the next segment with its step value + 1 to spawn. 0 is the initial step")]
    public int ChainStep;

    [Header("Animation")]
    [Tooltip("If enabled, the children of the segment will be spawned in reverse order of children")]
    public bool ReverseAnimation;

    [Tooltip("The time of delay there will be between each child spawn")]
    public float AnimationDelay = 0.1f;

    private void Start()
    {
        //Hide segment unless it's the first one
        if (ChainStep != 0)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        //Return if there are any children
        foreach (var _ in transform)
            return;

        //Destroying the object triggers the next chain to spawn
        Destroy(gameObject);
    }

    /// <summary>
    /// Plays a spawning animation for this segment
    /// </summary>
    public void SpawnSegment()
    {
        //Get (and possibly reverse) child collection
        var children = transform.Cast<Transform>();

        if (ReverseAnimation)
            children = children.Reverse();

        //Disable all children, and enable self
        foreach (var i in children)
            i.gameObject.SetActive(false);

        gameObject.SetActive(true);

        StartCoroutine(CoSpawnSegment());

        IEnumerator CoSpawnSegment()
        {
            //Reenable all children in order
            foreach (var i in children)
            {
                i.gameObject.SetActive(true);
                yield return new WaitForSeconds(AnimationDelay);
            }
        }
    }

    private void OnApplicationQuit()
    {
        _applicationInShutdown = true;
    }

    private void OnDestroy()
    {
        if (_applicationInShutdown)
            return;

        //Find and spawn the next chain segment
        var nextSegment = FindObjectsOfType<PickupChainSegment>(includeInactive: true)
            .Where(i => i.ChainGroup == ChainGroup && i.ChainStep == ChainStep + 1)
            .SingleOrDefault();

        if (nextSegment)
            nextSegment.SpawnSegment();
    }
}
