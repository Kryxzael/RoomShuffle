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

        [Tooltip("Which audio chanel should be used to play the sound. Each chanel has two audio sources")]
        public AudioChanel Chanel;
        
        [Tooltip("If true, the audio chanel will play the first and second clip in the list. This overrides clipIndex")]
        public bool PlayBoth = false;

        [Header("When to play")] 
        public bool _OnEnable;
        public bool _OnStart;
        public bool _OnDisable;
        public bool _OnDestroy;

        [Tooltip("What clip should be automatically played")]
        public int ClipIndex;

        private AudioSource _audioSource;
        private AudioSource _secondary;

        private void Awake()
        {
            //Get audiosources
            //Transform audioChannelObject = Commons.AudioManager.transform.Cast<Transform>()
            //    .FirstOrDefault(x => x.name.Equals(Chanel.ToString()));

            foreach (Transform child in Commons.AudioManager.transform)
            {
                if (child.name.Equals(Chanel.ToString()))
                {
                    _audioSource = child.GetComponent<AudioSource>();
                    _secondary = child.GetComponentInChildren<AudioSource>();
                    break;
                }
            }


        }

        /// <summary>
        /// Plays a sound from an audio chanel. If index is -1, it picks a random clip
        /// </summary>
        /// <param name="index"></param>
        public void PlaySound(int index = -1, float pitch = 1f)
        {
            if (!AudioClips.Any())
                return;

            _audioSource.pitch = pitch;
            _secondary.pitch = pitch;

            //If the two first sound should be played simultaneously
            if (PlayBoth && AudioClips[0] && AudioClips[1])
            {
                _audioSource.PlayOneShot(AudioClips[0]);
                _secondary.PlayOneShot(AudioClips[1]);
                return;
            }

            // if the index isn't set. set the index to a random clip
            if (index == -1)
            {
                index = new RandomValueBetween(0, AudioClips.Count-1).PickInt();
            }

            if (AudioClips[index] != null)
            {
                if (!_audioSource.isPlaying)
                {
                    //Make primary channel play sound
                    _audioSource.PlayOneShot(AudioClips[index]);
                }
                else
                {
                    //make secondary chanel make sound if primary is occupied
                    _secondary.PlayOneShot(AudioClips[index]);
                }
            }
        }

        private void Start()
        {
            if (_OnStart)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnEnable()
        {
            if (_OnEnable)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnDestroy()
        {
            if (_OnDestroy)
            {
                PlaySound(ClipIndex);
            }
        }
        
        private void OnDisable()
        {
            if (_OnDisable)
            {
                PlaySound(ClipIndex);
            }
        }
    }

    public enum AudioChanel
    {
        Pickup,
        DeniedAction,
        Fanfare,
        RedCoinsTick,
        RoomTimerTick,
        CurrencyHUD,
        Lock,
        ContactBlock,
    }
}
