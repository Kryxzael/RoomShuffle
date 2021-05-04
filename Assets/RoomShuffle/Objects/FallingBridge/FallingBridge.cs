using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// An object that falls when something steps on it
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FallingBridge : MonoBehaviour
{
    //Holds the initial state of the bridge's transform before it drops, so that it can respawn
    private StoredTransform _respawnConfig;

    /// <summary>
    /// Whether the bridge has fallen
    /// </summary>
    public bool Fallen { get; private set; }

    /* *** */

    private Rigidbody2D _rigidbody;

    /* *** */

    [Tooltip("The amount of seconds the object will wait before collapsing")]
    public float FallDelay = 0.25f;

    [Tooltip("The amount of seconds the object will wait before respawning")]
    public float RespawnDelay = 5f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _respawnConfig = new StoredTransform(transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Do not trigger if the bridge is already fallen or the object is moving upwards through it (semi-solidity)
        if (Fallen || Cheats.NoTarget || collision.relativeVelocity.y > 0.05f)
            return;

        Fallen = true;
        Invoke(nameof(MakeFall), FallDelay);
    }

    /// <summary>
    /// Makes the bridge fall
    /// </summary>
    public void MakeFall()
    {
        //Create a random rotation force
        const float MAX_RANDOM_TORQUE = 10f;
        float randomTorque = UnityEngine.Random.Range(-MAX_RANDOM_TORQUE, MAX_RANDOM_TORQUE);

        //Makes the object fall
        Fallen = true;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.AddTorque(randomTorque);

        //Schedule object to respawn
        Invoke(nameof(Respawn), RespawnDelay);
    }

    /// <summary>
    /// Resets the bridge to its original location
    /// </summary>
    public void Respawn()
    {
        Fallen = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;
        _respawnConfig.CopyTo(transform);
    }

    /// <summary>
    /// Stores the position, rotation and scale of an object. Unity transforms cannot exist outside of objects, so this is a custom one.
    /// </summary>
    private struct StoredTransform
    {
        /// <summary>
        /// The transform's global position
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The transform's global rotation
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// The transform's global scale
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Stores the provided transform
        /// </summary>
        /// <param name="copyFrom"></param>
        public StoredTransform(Transform copyFrom)
        {
            Position = copyFrom.position;
            Rotation = copyFrom.rotation;
            Scale = copyFrom.localScale;
        }

        /// <summary>
        /// Copies the components of this stored transform to the provided Unity transform instance
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(Transform target)
        {
            target.position = Position;
            target.rotation = Rotation;
            target.localScale = Scale;
        }
    }
}