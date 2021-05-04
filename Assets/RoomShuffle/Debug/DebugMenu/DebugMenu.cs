using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Renders a debug menu that can be navigated with the arrow keys or D-pad
/// </summary>
public class DebugMenu : MonoBehaviour
{
    /// <summary>
    /// Gets or sets whether the debug menu is currently visible
    /// </summary>
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets the current navigation stack of debug pages
    /// </summary>
    public Stack<DebugPage> NavigationStack { get; } = new Stack<DebugPage>();

    //The current run output
    private List<string> _currentRun = new List<string>();

    [Tooltip("Items that can be spawned through debug mode")]
    public List<GameObject> SpawnableItems;

    /// <summary>
    /// Gets the current debug page
    /// </summary>
    public DebugPage CurrentPage
    {
        get => NavigationStack.Peek();
    }

    //Whether a debug button was pressed last frame
    private bool lastFrameHeldButton;

    private void Update()
    {

        //If the navigation stack somehow is empty, return to home
        if (!NavigationStack.Any())
            NavigationStack.Push(new HomeDebugPage());

        //Gets the current run page
        _currentRun = CurrentPage.Run(this, false);

        /*
         * Input
         */

        //Click Down
        if (Input.GetAxisRaw("DebugVertical") == -1)
        {
            if (lastFrameHeldButton || !IsOpen)
                return;

            CurrentPage.SelectedIndex = Mathf.Min(++CurrentPage.SelectedIndex, _currentRun.Count - 1);
        }

        //Click Up
        else if (Input.GetAxisRaw("DebugVertical") == 1)
        {
            if (lastFrameHeldButton || !IsOpen)
                return;

            CurrentPage.SelectedIndex = Mathf.Max(--CurrentPage.SelectedIndex, 0);
        }

        //Click Select (Right)
        else if (Input.GetAxisRaw("DebugHorizontal") == 1)
        {
            if (lastFrameHeldButton)
                return;

            if (!IsOpen)
                IsOpen = true;

            else
                _currentRun = CurrentPage.Run(this, true);
        }

        //Click Back (Left)
        else if (Input.GetAxisRaw("DebugHorizontal") == -1)
        {
            if (lastFrameHeldButton)
                return;

            if (NavigationStack.Count > 1)
                NavigationStack.Pop();
            else
                IsOpen = false;

            _currentRun = CurrentPage.Run(this, false);
        }

        //No input
        else
        {
            lastFrameHeldButton = false;
            return;
        }

        lastFrameHeldButton = true;
    }

    private void OnGUI()
    {
        //Debug menu is hidden. So don't show it
        if (!IsOpen)
            return;

        //Draws the list
        DrawList(new string[] { "==DEBUG MODE==", CurrentPage.Header, "" }.Concat(_currentRun).ToArray(), NavigationStack.Peek().SelectedIndex);
    }

    /// <summary>
    /// Draws a list of strings to the top right corner of the screen
    /// </summary>
    /// <param name="texts"></param>
    private void DrawList(string[] texts, int index)
    {
        //How many lines to display (excluding header)
        const int DISPLAYED_LINES = 15;

        //Size of the header
        const int HEADER_SIZE = 3;

        //For each line to draw (always 0 --> maxiumum of DISPLAYED LINES + HEADER SIZE)
        for (int i = 0; i < Mathf.Min(texts.Length, DISPLAYED_LINES + HEADER_SIZE); i++)
        { 
            //The menu's offset from the top of the screen
            const int VERTICAL_OFFSET = 125;

            //The offset between each line
            const float VERTICAL_PADDING = 20f; 

            //By how much the shadow drawn under the text will be offset
            const float SHADOW_OFFSET = 2.5f;

            /*
             * Adjust index for scrolling 
             */
            int adjustedIndex = i;

            if (index >= DISPLAYED_LINES)
                adjustedIndex = i + index + HEADER_SIZE - DISPLAYED_LINES - 2;

            if (adjustedIndex >= texts.Length)
                break;

            /*
             * Draw Shadow
             */
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
                style.normal.textColor = Color.black;

                GUI.Label(new Rect(SHADOW_OFFSET, i * VERTICAL_PADDING + SHADOW_OFFSET + VERTICAL_OFFSET, Screen.width, VERTICAL_PADDING), texts[adjustedIndex], style);
            }

            /*
             * Draw Text
             */
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
                style.normal.textColor = Color.white;

                GUI.Label(new Rect(0f, i * VERTICAL_PADDING + VERTICAL_OFFSET, Screen.width, VERTICAL_PADDING), texts[adjustedIndex], style);
            }
        }
    }
}
