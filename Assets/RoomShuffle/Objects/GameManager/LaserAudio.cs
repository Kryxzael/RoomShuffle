using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles the audio created by lasers
/// </summary>
public class LaserAudio : MonoBehaviour
{
    //Holds the lasers that are currently on-screen
    private HashSet<GameObject> _activeLasers = new HashSet<GameObject>();

    /* *** */

    private AudioSource _audioSource;


    void Start()
    {
        _audioSource = transform.Cast<Transform>()
            .FirstOrDefault(x => x.name.Equals("Laser"))
            .GetComponent<AudioSource>();
    }

    void Update()
    {
        //Play laser sound if at least one laser is on-screen
        if (_activeLasers.Count > 0 && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        //Stop laser sound if no lasers are on-screen
        else if (_activeLasers.Count == 0 && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    /// <summary>
    /// Registers a laser as being on-screen
    /// </summary>
    /// <param name="laser"></param>
    public void AddLaser(GameObject laser)
    {
        _activeLasers.Add(laser);
    }

    /// <summary>
    /// Deregisters a laser
    /// </summary>
    /// <param name="laser"></param>
    public void RemoveLaser(GameObject laser)
    {
        _activeLasers.Remove(laser);
    }
}