using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns an arrangement of currency pickups
/// </summary>
public class CurrencySpawner : MonoBehaviour
{
    private void Start()
    {
        
    }


    private static IEnumerable<Vector2> CreateSolidBlock(Vector2 size)
    {
        return FilterBox(size, (x, y) => true);
    }

    private static IEnumerable<Vector2> CreateHollowBlock(Vector2 size)
    {
        return FilterBox(size, (x, y) => x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1);
    }

    private static IEnumerable<Vector2> CreateCheckerBoard(Vector2 size)
    {
        return FilterBox(size, (x, y) => x % 2 != y % 2);
    }

    private static IEnumerable<Vector2> CreateVerticalStripes(Vector2 size)
    {
        return FilterBox(size, (x, y) => y % 2 == 0);
    }

    private static IEnumerable<Vector2> CreateHorizontalStripes(Vector2 size)
    {
        return FilterBox(size, (x, y) => x % 2 == 0);
    }

    private static IEnumerable<Vector2> CreateSingle(Vector2 size)
    {
        return FilterBox(size, (x, y) => x == size.x / 2f && y == size.y / 2f);
    }

    private static IEnumerable<Vector2> CreateCircle(Vector2 size)
    {
        return FilterBox(size, (x, y) => x == size.x / 2f && y == size.y / 2f);
    }

    private static IEnumerable<Vector2> FilterBox(Vector2 size, Func<int, int, bool> shouldInclude)
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; x < size.y; y++)
                if (shouldInclude(x, y))
                    yield return new Vector2(x, y);
    }
}
