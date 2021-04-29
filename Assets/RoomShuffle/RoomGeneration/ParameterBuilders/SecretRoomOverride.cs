using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// A parameter picker that generates secret rooms
/// </summary>
[CreateAssetMenu(menuName = "Room Parameter Builders/Secret")]
public class SecretRoomOverride : ParameterBuilderOverride
{
    public bool HasGenerated { get; private set; }

    public override RoomParameters GetInitialParameters(System.Random random)
    {
        HasGenerated = true;

        return new RoomParameters()
        {
            Class = RoomClass.Secret,
            Effect = RoomEffects.None,
            Entrance = EntranceExitSides.Left,
            Exit = EntranceExitSides.Right,
            FlipHorizontal = random.Next(2) == 0,
            Layout = Rooms.SecretRooms[random.Next(Rooms.SecretRooms.Count)],
            Theme = RoomTheme.Autumn,
            WeaponEnumerator = null
        };
    }

    public override RoomParameters GetNextParameters(RoomHistory history, System.Random random)
    {
        return GetInitialParameters(random);
    }

    public override bool HasNext()
    {
        return !HasGenerated;
    }
}