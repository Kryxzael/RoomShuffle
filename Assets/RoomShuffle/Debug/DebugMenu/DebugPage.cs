using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for pages displayable with a debug menu component
/// </summary>
public abstract class DebugPage
{
    /// <summary>
    /// Gets the header to use on the display page
    /// </summary>
    public abstract string Header { get; }

    /// <summary>
    /// Gets the page's currently selected item index
    /// </summary>
    public int SelectedIndex { get; set; }

    /*
     * Mid-run properties
     */

    //The index of the last drawn run-item
    private int _currentRunIndex;

    //Whether the current or last run contains a selection prompt
    private bool _currentRunClicking;

    //The output strings of the current or last run
    private List<string> _currentRunOutput;

    /// <summary>
    /// Runs the debug page and returns the associated strings to draw
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="click"></param>
    /// <returns></returns>
    public List<string> Run(DebugMenu caller, bool click)
    {
        _currentRunOutput = new List<string>();
        _currentRunIndex = -1;
        _currentRunClicking = click;

        RunItems(caller);

        return _currentRunOutput;
    }

    /// <summary>
    /// Creates a run of items
    /// </summary>
    /// <param name="caller"></param>
    protected abstract void RunItems(DebugMenu caller);

    /// <summary>
    /// Creates a clickable button in a run. When placed in an if statement, will run the if block if the item is clicked
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    protected bool Button(string label)
    {
        //Increases what line we are at
        _currentRunIndex++;

        //Draw label
        if (SelectedIndex == _currentRunIndex)
            _currentRunOutput.Add($">> {label} <<");
        else
            _currentRunOutput.Add(label);

        //Return whether the button was clicked, allowing buttons to be placed in if-s
        return _currentRunClicking && _currentRunIndex == SelectedIndex;
    }

    /// <summary>
    /// Creates a togglable button in a run. When placed in an if statement, will run the if block if the item is clicked
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    protected bool Toggle(string label, bool enabled)
    {
        if (enabled)
            return Button($"{label} [x]");
        else
            return Button($"{label} [ ]");
    }

    /// <summary>
    /// Creates a label in a run.
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    protected void ReadOnly(string label)
    {
        //Increases what line we are at
        _currentRunIndex++;

        //Draw label
        if (SelectedIndex == _currentRunIndex)
            _currentRunOutput.Add($"[[ {label} ]]");
        else
            _currentRunOutput.Add(label);
    }

    /// <summary>
    /// Creates a one-line separator in a run
    /// </summary>
    protected void Separator()
    {
        ReadOnly(" ");
    }
}
