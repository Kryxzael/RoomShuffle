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

    public int EnemySetIndex = 0;
    public int WeaponSetIndex = 0;

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

        return new RoomParameters()
        {
            Class = RoomClass.Platforming,
            Layout = _currentRoom.Current,
            Theme = RoomTheme.Grass,
            Effect = RoomEffects.None,
            EnemySet = EnemySets[EnemySetIndex],
            FlipHorizontal = false,
            WeaponEnumerator = Enumerable.Repeat(WeaponTemplates.ElementAt(WeaponSetIndex), 100).GetEnumerator()
        };
    }
}
