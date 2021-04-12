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
        private float _phaseOffset;

        /* *** */

        [Tooltip("If enabled, the bobbing will be horizontal")]
        public bool Horizontal;

        [Tooltip("How far (up and down) the item will bob")]
        public float BobHeight = 0.25f;

        [Tooltip("How fast the item will bob")]
        public float BobSpeed = 2f;

        [Tooltip("The offeset of the bobbing")]
        [Range(0, 1)]
        public float MaxOffset = 1f;

        [Tooltip("If the element should select a random value between 0 and MaxOffset")]
        public bool UseRandomBobbing = true;

        private void Start()
        {
            _startingPosition = transform.localPosition;
            if (UseRandomBobbing)
            {
                _phaseOffset = UnityEngine.Random.Range(0, MaxOffset * Mathf.PI * 2f);
            }
            else
            {
                _phaseOffset = MaxOffset * MaxOffset * Mathf.PI * 2f;
            }
            
        }

        private void Update()
        {
            Vector3 direction = Horizontal ? transform.right : transform.up;

            transform.localPosition = _startingPosition + direction * (Mathf.Sin((_time + _phaseOffset) * BobSpeed) * BobHeight);
            _time += Time.deltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            float distance = BobHeight;

            if (Horizontal)
            {
                Gizmos.DrawLine(transform.position + Vector3.right * distance, transform.position + Vector3.left * distance);
            }
            else
            {
                Gizmos.DrawLine(transform.position + Vector3.up * distance, transform.position + Vector3.down * distance);
            }
        }
    }
}
