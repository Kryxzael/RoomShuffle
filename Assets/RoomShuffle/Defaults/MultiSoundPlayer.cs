using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoomShuffle.Defaults

{
    public class MultiSoundPlayer : MonoBehaviour
    {
    
        [Tooltip("List of Audio clips that can be played")]
        public List<AudioClip> AudioClips = new List<AudioClip>();

        [Tooltip("If true, the audio source will play all the clips at the same time!")]
        public bool PlayAll = false;

        [Tooltip("The minimum amount of time in seconds between each sound")]
        [Range(0, 1)]
        public float MinimumTimeBetweenSounds = 0;

        public bool LimitAudio;

        [Header("When to play")] 
        public bool PlayOnEnable;
        public bool PlayOnStart;
        public bool PlayOnDisable;
        public bool PlayOnDestroy;
        public bool PlayOnUpdate; 

        [Tooltip("What clip should be automatically played on the 'PlayOn____' options")]
        public int ClipIndex;

        //Used for all non pitch corrected audio
        private AudioSource _primaryAudioSource;
        
        //only used for pitch adjusted audio
        private AudioSource _secondaryAudioSource;

        private float _timePassed;

        private LimitedAudioManager _limitedAudioManager;

        private void Awake()
        {
            GameObject AudioManager = Commons.AudioManager;
            
            //get audio source
            _primaryAudioSource = AudioManager.GetComponent<AudioSource>();
            _secondaryAudioSource = AudioManager.transform.Cast<Transform>().FirstOrDefault().GetComponent<AudioSource>();
            _limitedAudioManager = AudioManager.GetComponent<LimitedAudioManager>();
        }

        private void Update()
        {
            if (_timePassed <= MinimumTimeBetweenSounds)
                _timePassed += Time.deltaTime;
            
            if (PlayOnUpdate)
                PlaySound(ClipIndex);
        }

        /// <summary>
        /// Plays a sound from an audio channel. If index is -1, it picks a random clip
        /// </summary>
        /// <param name="index"></param>
        public void PlaySound(int index = -1, float pitch = 1f, float volume = 1f)
        {
            if (!AudioClips.Any())
                return;

            if (_timePassed < MinimumTimeBetweenSounds)
            {
                return;
            }

            //Set audiosource according to pitch. Use secondary source only if pitch is different
            AudioSource source = pitch == 1f ? _primaryAudioSource : _secondaryAudioSource;
            
            _timePassed = 0;

            source.pitch = pitch;

            //Play all the clips
            if (PlayAll)
            {
                foreach (AudioClip clip in AudioClips)
                {
                    if (LimitAudio)
                    {
                        _limitedAudioManager.PlayLimitedSound(clip);
                    }
                    else
                    {
                        source.PlayOneShot(clip, volume);   
                    }
                }
                return;
            }

            // if the index isn't set. set the index to a random clip
            if (index == -1)
            {
                index = new RandomValueBetween(0, AudioClips.Count).PickInt();
            }

            if (AudioClips[index])
            {
                if (LimitAudio)
                {
                    _limitedAudioManager.PlayLimitedSound(AudioClips[index]);
                }
                else
                {
                    source.PlayOneShot(AudioClips[index], volume);
                }
                
            }
        }

        private void Start()
        {
            if (PlayOnStart)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnEnable()
        {
            if (PlayOnEnable)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnDestroy()
        {
            if (PlayOnDestroy)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnDisable()
        {
            if (PlayOnDisable)
            {
                PlaySound(ClipIndex);
            }
        }
    }
}
