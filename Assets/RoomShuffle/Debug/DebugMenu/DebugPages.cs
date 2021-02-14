using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.RoomShuffle.Debug.DebugMenu
{
    public static class DebugPages
    {
        public static DebugPage Home = new DebugPage("Home")
        {
            new ToggleDebugItem("Moon Jump", _ => Cheats.MoonJump = !Cheats.MoonJump, () => Cheats.MoonJump),
            new DebugItem("Room Generation", nav => nav.Push(new DebugPage("Room Generation")
            {
                new DebugItem("Generate Next", _ => Object.FindObjectOfType<RoomGenerator>().GenerateNext())
            }))
        };
    }
}
