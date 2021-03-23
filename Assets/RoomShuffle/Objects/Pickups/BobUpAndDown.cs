using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.RoomShuffle.Objects.Pickups
{
    /// <summary>
    /// Makes an object bob slightly up and down
    /// </summary>
    public class BobUpAndDown : MonoBehaviour
    {
        //Keeps track of the object starting position (which will be offset)
        private Vector3 _startingPosition;

        //The time the object has existed for
        private float _time;

        //A random offset in the bobbing phase so that items don't bob in sync
        private float _randomPhaseOffset;

        /* *** */

        [Tooltip("How far (up and down) the item will bob")]
        public float BobHeight = 0.25f;

        [Tooltip("How fast the item will bob")]
        public float BobSpeed = 2f;

        private void Start()
        {
            _startingPosition = transform.position;
            _randomPhaseOffset = UnityEngine.Random.Range(0, Mathf.PI * 2f);
        }

        private void Update()
        {
            transform.position = _startingPosition + (Vector3.up * Mathf.Sin((_time + _randomPhaseOffset) * BobSpeed) * BobHeight);
            _time += Time.deltaTime;
        }
    }
}
