using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Allows an object to play a sound on the sound manager
/// </summary>
public class MultiSoundPlayer : MonoBehaviour
{
    //The amount of time that has passed since the last playback
    private float _timeSinceLastPlayback;

    //Used for all non pitch corrected audio
    private AudioSource _primaryAudioSource;

    //only used for pitch adjusted audio
    private AudioSource _secondaryAudioSource;


    /* *** */

    private LimitedAudioManager _limitedAudioManager;

    /* *** */

    [Tooltip("List of Audio clips that can be played. By default, a random audio clip will be selected from this list")]
    public List<AudioClip> AudioClips = new List<AudioClip>();

    [Tooltip("If true, the audio source will play all the clips at the same time, rather than a random one")]
    public bool PlayAll = false;

    [Tooltip("The minimum amount of time in seconds between each sound. Any subsequent play call within this timeframe will be ignored")]
    [Min(0f)]
    public float MinimumTimeBetweenSounds = 0;

    [Tooltip("Plays the sounds through the limited audio script")]
    public bool LimitAudio;


    /*
     * Playback timing
     */

    [Header("When to play")] 
    [Tooltip("If enabled, a sound will be played when the script is enabled")]
    public bool PlayOnEnable;

    [Tooltip("If enabled, a sound will be played when the script starts")]
    public bool PlayOnStart;

    [Tooltip("If enabled, a sound will be played when the script is enabled")]
    public bool PlayOnDisable;

    [Tooltip("If enabled, a sound will be played when the script is destroyed")]
    public bool PlayOnDestroy;

    [Tooltip("If enabled, a sound will be played every frame. Should only be used when MinimumTimeBetweenSOunds is greater than 0")]
    public bool PlayOnUpdate; 

    [Tooltip("What clip should be automatically played on the 'PlayOn____' options")]
    public int ClipIndex;

    private void Awake()
    {
        //Get audio sources
        _primaryAudioSource = Commons.AudioManager.GetComponent<AudioSource>();
        _secondaryAudioSource = Commons.AudioManager.transform.Cast<Transform>().FirstOrDefault().GetComponent<AudioSource>();
        _limitedAudioManager = Commons.AudioManager.GetComponent<LimitedAudioManager>();
    }

    private void Update()
    {
        //Increase time since last playback
        if (_timeSinceLastPlayback <= MinimumTimeBetweenSounds)
            _timeSinceLastPlayback += Time.deltaTime;
            
        //If a sound is due to be played now, play it
        if (PlayOnUpdate)
            PlaySound(ClipIndex);
    }

    /// <summary>
    /// Plays a sound from an audio channel. If index is -1, it picks a random clip
    /// </summary>
    /// <param name="index"></param>
    public void PlaySound(int index = -1, float pitch = 1f, float volume = 1f)
    {
        //No audio clips are present. Log a warning
        if (!AudioClips.Any())
            return;

        //Sound cannot be played because of minimum time between seconds
        if (_timeSinceLastPlayback < MinimumTimeBetweenSounds)
            return;

        //Reset play time
        else
            _timeSinceLastPlayback = 0;

        //Setup audiosource according to pitch. Use secondary source only if pitch is different
        AudioSource source = pitch == 1f ? _primaryAudioSource : _secondaryAudioSource;
        source.pitch = pitch;

        //Play all the clips if configured to do so
        if (PlayAll)
        {
            foreach (AudioClip clip in AudioClips)
            {
                if (LimitAudio)
                    _limitedAudioManager.PlayLimitedSound(clip);

                else
                    source.PlayOneShot(clip, volume);
            }
        }

        //Play a single sound
        else
        {
            //If the index isn't set. Set the index to a random clip
            if (index == -1)
                index = UnityEngine.Random.Range(0, AudioClips.Count);

            //Play clip
            if (AudioClips[index])
            {
                if (LimitAudio)
                    _limitedAudioManager.PlayLimitedSound(AudioClips[index]);

                else
                    source.PlayOneShot(AudioClips[index], volume);
            }

            //Clip was invalid
            else
            {
                Debug.LogWarning("Attempted to play sound-clip that does not exist");
            }
        }
    }

    private void Start()
    {
        //If a sound is due to play now, play it.
        if (PlayOnStart)
        {
            PlaySound(ClipIndex);
        }
    }
        
    private void OnEnable()
    {
        //If a sound is due to play now, play it.
        if (PlayOnEnable)
        {
            PlaySound(ClipIndex);
        }
    }
        
    private void OnDestroy()
    {
        //If a sound is due to play now, play it.
        if (PlayOnDestroy)
        {
            PlaySound(ClipIndex);
        }
    }
        
    private void OnDisable()
    {
        //If a sound is due to play now, play it.
        if (PlayOnDisable)
        {
            PlaySound(ClipIndex);
        }
    }
}
