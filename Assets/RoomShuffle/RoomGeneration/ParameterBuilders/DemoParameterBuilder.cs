using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(menuName = "Room Parameter Builders/Demo")]
public class DemoParameterBuilder : ParameterBuilder
{
    private IEnumerator<RoomLayout> _currentRoom;

    public List<RoomLayout> DemoRoomProgression;

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
        output.Class = RoomClass.Platforming;
        output.Layout = _currentRoom.Current;
        output.Theme = typeof(RoomTheme).GetEnumValues() //Absolutely disgusting, but it's for testing so it's ok
            .Cast<RoomTheme>()
            .Where(i => i != RoomTheme.Edit)
            .OrderBy(i => random.Next())
            .First();

        output.EnemySet = EnemySets[random.Next(EnemySets.Count)];
        output.Effect = RoomEffects.None;
        output.FlipHorizontal = random.Next(2) == 0;
        output.WeaponEnumerator = WeaponTemplates.OrderBy(i => random.Next()).GetEnumerator();
        output.Entrance = output.Layout.GetRandomEntrance(random);

        do
        {
            output.Exit = output.Layout.GetRandomExit(random, EntranceExitSides.None);
        } while (output.Entrance == output.Exit);

        return output;
    }
}
