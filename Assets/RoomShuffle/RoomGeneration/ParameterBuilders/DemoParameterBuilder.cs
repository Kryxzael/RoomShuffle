using Assets.RoomShuffle.Objects.HUD;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(menuName = "Room Parameter Builders/Demo")]
public class DemoParameterBuilder : ParameterBuilder
{
    private IEnumerator<DemoRoomConfig> _currentRoom;

    [Header("The list of demo rooms")]
    public List<DemoRoomConfig> DemoRoomProgression;

    public override RoomParameters GetInitialParameters(System.Random random)
    {
        return GetNextParameters(null, random);
    }

    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        if (_currentRoom == null || !_currentRoom.MoveNext())
        {
            _currentRoom = DemoRoomProgression.GetEnumerator();
            _currentRoom.MoveNext();
        }

        var output = new RoomParameters();

        output.GroundEnemies = GroundEnemies;
        output.AirEnemies = AirEnemies;
        output.Class = _currentRoom.Current.Class;
        output.Layout = _currentRoom.Current.Room;
        output.Theme = _currentRoom.Current.Theme;

        output.Effect = _currentRoom.Current.Effects;
        output.FlipHorizontal = _currentRoom.Current.FlipHorizontal;
        output.WeaponEnumerator = WeaponTemplates.OrderBy(i => random.Next()).GetEnumerator();
        output.Entrance = output.Layout.GetRandomEntrance(random);

        do
        {
            output.Exit = output.Layout.GetRandomExit(random, EntranceExitSides.None);
        } while (output.Entrance == output.Exit && output.Layout.EntranceSides != output.Layout.ExitSides);

        //Override gameplay settings
        FindObjectOfType<DemoHUDManager>().SetUIFlags(_currentRoom.Current.HideUI);
        Commons.SoundtrackPlayer.MuteMaster = _currentRoom.Current.MuteMusic;
        Cheats.HealthCheat = _currentRoom.Current.HealthCheat;

        return output;
    }

    [Serializable]
    public class DemoRoomConfig
    {
        public RoomClass Class;
        public RoomLayout Room;
        public RoomEffects Effects;
        public RoomTheme Theme;
        public bool FlipHorizontal;
        public bool MuteMusic;
        public Cheats.HealthCheatType HealthCheat;
        public UIHideFlags HideUI;

        [Flags]
        public enum UIHideFlags
        {
            None = 0x0,
            AttackLevel = 0x1,
            Health = 0x2,
            Weapons = 0x4,
            Item = 0x8,
            WeaponStats = 0x10,
            RoomNumber = 0x20,
            Currency = 0x40,
            GeneralKeys = 0x80,
            PuzzleKeys = 0x100,
            EscapeToExit = 0x200,
            RoomClass = 0x400,
            RoomEffect = 0x800,
        }
    }
}
