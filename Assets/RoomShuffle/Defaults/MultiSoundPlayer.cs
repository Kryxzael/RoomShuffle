using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoomShuffle.Defaults

{
    public class MultiSoundPlayer : MonoBehaviour
    {
    
        public List<AudioClip> AudioClips = new List<AudioClip>();

        public AudioChanel Chanel;

        [Header("When to play")] 
        public bool _OnEnable;
        public bool _OnStart;
        public bool _OnDisable;
        public bool _OnDestroy;

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

        public void PlaySound(int index = -1)
        {
            if (!AudioClips.Any())
                return;

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
        CurrencyHUD
    }
}
