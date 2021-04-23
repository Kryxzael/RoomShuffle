using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LimitedAudioManager : MonoBehaviour
{
    [Range(0,1)]
    public float MinimumTimeBetweenSound;
    
    private float _timePassed;
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = transform.Cast<Transform>().FirstOrDefault(x => x.name.Equals("Projectile"))
            .GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_timePassed <= MinimumTimeBetweenSound)
            _timePassed += Time.deltaTime;
    }

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
