using System.Linq;
using UnityEngine;

/// <summary>
/// An audio-manager that will only play a sound if no other sounds have played within a certain time-window
/// </summary>
public class LimitedAudioManager : MonoBehaviour
{
    //How much time has passed since the last successful sound play
    private float _timePassed;

    /* *** */

    private AudioSource _audioSource;

    /* *** */

    [Range(0, 1)]
    [Tooltip("The amount of time that must pass between two sound plays")]
    public float MinimumTimeBetweenSound;

    void Start()
    {
        _audioSource = transform.Cast<Transform>()
            .FirstOrDefault(x => x.name.Equals("Projectile"))
            .GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_timePassed <= MinimumTimeBetweenSound)
            _timePassed += Time.deltaTime;
    }

    /// <summary>
    /// Attempts to play a limited sound
    /// </summary>
    /// <param name="clip"></param>
    public void PlayLimitedSound(AudioClip clip)
    {
        if (_timePassed < MinimumTimeBetweenSound)
        {
            return;
        }

        _timePassed = 0;

        _audioSource.pitch = new RandomValueBetween(1.3f, 1.6f).Pick();
        _audioSource.PlayOneShot(clip, 0.4f);
    }
}
