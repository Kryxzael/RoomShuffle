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
        _currentRoom = DemoRoomProgression.GetEnumerator();
        return GetNextParameters(null, random);
    }

    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        if (!_currentRoom.MoveNext())
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
    }
}
