using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserAudio : MonoBehaviour
{
    private AudioSource _audioSource;

    private HashSet<GameObject> _activeLasers = new HashSet<GameObject>();

    void Start()
    {
        _audioSource = transform.Cast<Transform>().FirstOrDefault(x => x.name.Equals("Laser"))
            .GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_activeLasers.Count > 0 && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        else if (_activeLasers.Count == 0 && _audioSource.isPlaying )
        {
            _audioSource.Stop();
        }
    }

    public void AddLaser(GameObject laser)
    {
        _activeLasers.Add(laser);
    }

    public void RemoveLaser(GameObject laser)
    {
        _activeLasers.Remove(laser);
    }
}