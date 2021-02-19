using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents a log of generated rooms
/// </summary>
public class RoomHistory : IEnumerable<RoomParameters>
{
    private Stack<RoomParameters> _history = new Stack<RoomParameters>();

    /// <summary>
    /// Registers the provided room in the history
    /// </summary>
    /// <param name="room"></param>
    public void RegisterHistory(RoomParameters room)
    {
        _history.Push(room);
    }

    /// <summary>
    /// Gets the amount of rooms there have been since the last occurrence of the provided class
    /// </summary>
    /// <param name="class"></param>
    /// <returns></returns>
    public int RoomsSinceClass(RoomClass @class)
    {
        return RoomsSinceMatchOfPredicate(i => i.Class == @class);
    }

    /// <summary>
    /// The amount of rooms there have been since the theme changed
    /// </summary>
    /// <returns></returns>
    public int RoomsSinceThemeChange()
    {
        if (!this.Any())
            return 0;

        RoomParameters last = this.First();

        return RoomsSinceMatchOfPredicate(i => i.Theme != last.Theme);
    }

    /// <summary>
    /// Counts how many rooms have been generated since the last room that matches the provided predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public int RoomsSinceMatchOfPredicate(Func<RoomParameters, bool> predicate)
    {
        return _history.TakeWhile(i => !predicate(i)).Count();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public IEnumerator<RoomParameters> GetEnumerator()
    {
        return _history.GetEnumerator();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _history.GetEnumerator();
    }
}
