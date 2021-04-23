using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows an object to create an explosion of rigidbodies
/// </summary>
public class ParticleExplosion : MonoBehaviour
{
    [Tooltip("The amount of rigidbodies to spawn when a big explosion is created")]
    public RandomValueBetween ParticleCount;

    [Tooltip("The amount of rigidbodies to spawn when a small explosion is created")]
    public RandomValueBetween SmallDropParticleCount;

    [Tooltip("The particles to spawn")]
    public List<Rigidbody2D> Particles;

    [Tooltip("The amount of force to apply to the rigidbody")]
    public RandomValueBetween LaunchForce;

    [Tooltip("The amount of torque to apply to the rigidbody")]
    public RandomValueBetween TorqueForce;

    /// <summary>
    /// Creates an explosion with at particle count determined by the ParticleCount field
    /// </summary>
    public void ExplodeBig()
    {
        Explode(ParticleCount.PickInt());
    }

    /// <summary>
    /// Creates an explosion with at particle count determined by the SmallDropParticleCount field
    /// </summary>
    public void ExplodeSmall()
    {
        Explode(SmallDropParticleCount.PickInt());
    }

    /// <summary>
    /// Creates an explosion with at particle count determined by the provided argument
    /// </summary>
    public void Explode(int particleCount)
    {
        for (int i = 0; i < particleCount; i++)
        {
            var particle = Commons.InstantiateInCurrentLevel(Particles[Random.Range(0, Particles.Count)], transform.position);
            particle.velocity = Random.insideUnitSphere * TorqueForce.Pick();
            particle.SetVelocityY(currentY => Mathf.Abs(currentY));

            particle.AddTorque(TorqueForce.Pick());
        }
    }
}
