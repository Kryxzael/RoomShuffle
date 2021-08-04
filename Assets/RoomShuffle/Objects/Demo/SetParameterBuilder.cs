using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.RoomShuffle.Objects.Demo
{
    public class SetParameterBuilder : MonoBehaviour
    {
        public ParameterBuilder Builder;

        private void Start()
        {
            Commons.RoomGenerator.RoomParameterBuilder = Builder;
            Commons.RoomGenerator.History.Clear();
        }
    }
}
