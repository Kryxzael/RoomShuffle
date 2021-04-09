using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyBlockerDisplay : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider2D>();

        Gizmos.color = Color.green;

        if (collider is BoxCollider2D)
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);

        else if (collider is CircleCollider2D)
            Gizmos.DrawWireSphere(collider.bounds.center, collider.bounds.size.magnitude / 2f);
    }
}
