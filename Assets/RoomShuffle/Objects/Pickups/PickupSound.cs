using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace RoomShuffle.Objects.Pickups
{
    
    [RequireComponent(typeof(MultiSoundPlayer))]
    public class PickupSound : PickupScript
    {
        public int ClipIndex;

        private MultiSoundPlayer _multiSoundPlayer;

        private void Awake()
        {
            _multiSoundPlayer = GetComponent<MultiSoundPlayer>();
        }
        

        public override void OnPickup()
        {
            _multiSoundPlayer.PlaySound(ClipIndex);
        }
    }
    
}