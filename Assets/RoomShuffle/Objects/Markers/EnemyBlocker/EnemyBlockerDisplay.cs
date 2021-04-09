using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[ExecuteInEditMode]
public class EnemyBlockerDisplay : MonoBehaviour
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (_collider is BoxCollider2D)
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);

        else if (_collider is CircleCollider2D)
            Gizmos.DrawWireSphere(_collider.bounds.center, _collider.bounds.size.magnitude / 2f);
    }
}
