using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// Changes the music when the player is within a trigger
/// </summary>
public class MusicZone : MonoBehaviour
{
    private static HashSet<MusicZone> _holders = new HashSet<MusicZone>();

    public SoundtrackPlayer.MusicChannels Channels;

    private void LateUpdate()
    {
        if (_holders.Contains(this))
            Commons.SoundtrackPlayer.OverrideChannels = Channels;

        else if (!_holders.Any())
            Commons.SoundtrackPlayer.OverrideChannels = SoundtrackPlayer.MusicChannels.None;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _holders.Add(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.IsPlayer())
            _holders.Remove(this);
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider2D>();

        Gizmos.color = Color.blue;

        if (collider is BoxCollider2D)
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);

        else if (collider is CircleCollider2D)
            Gizmos.DrawWireSphere(collider.bounds.center, collider.bounds.size.magnitude / 2f);
    }

    private void OnDestroy()
    {
        _holders.Remove(this);

        //Commons.SoundtrackPlayer is null if the game is reloading
        if (!_holders.Any() && Commons.SoundtrackPlayer)
            Commons.SoundtrackPlayer.OverrideChannels = SoundtrackPlayer.MusicChannels.None;
    }
}
