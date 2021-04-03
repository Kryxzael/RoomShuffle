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
    /// Gets the current navigation stack of debug pages
    /// </summary>
    public Stack<DebugPage> NavigationStack { get; } = new Stack<DebugPage>();

    //The current run output
    private List<string> _currentRun = new List<string>();

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
            if (lastFrameHeldButton)
                return;

            CurrentPage.SelectedIndex = Mathf.Min(++CurrentPage.SelectedIndex, _currentRun.Count - 1);
        }

        //Click Up
        else if (Input.GetAxisRaw("DebugVertical") == 1)
        {
            if (lastFrameHeldButton)
                return;

            CurrentPage.SelectedIndex = Mathf.Max(--CurrentPage.SelectedIndex, 0);
        }

        //Click Select (Right)
        else if (Input.GetAxisRaw("DebugHorizontal") == 1)
        {
            if (lastFrameHeldButton)
                return;

            _currentRun = CurrentPage.Run(this, true);
        }

        //Click Back (Left)
        else if (Input.GetAxisRaw("DebugHorizontal") == -1)
        {
            if (lastFrameHeldButton)
                return;

            if (NavigationStack.Count > 1)
                NavigationStack.Pop();

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
        //Draws the list
        DrawList(new string[] { "==DEBUG MODE==", CurrentPage.Header, "" }.Concat(_currentRun).ToArray());
    }

    /// <summary>
    /// Draws a list of strings to the top right corner of the screen
    /// </summary>
    /// <param name="texts"></param>
    private void DrawList(string[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            const int VERTICAL_OFFSET = 100;
            const float VERTICAL_PADDING = 20f;
            const float SHADOW_OFFSET = 2.5f;

            //Shadow
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
                style.normal.textColor = Color.black;

                GUI.Label(new Rect(SHADOW_OFFSET, i * VERTICAL_PADDING + SHADOW_OFFSET + VERTICAL_OFFSET, Screen.width, VERTICAL_PADDING), texts[i], style);
            }

            //Text
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
                style.normal.textColor = Color.white;

                GUI.Label(new Rect(0f, i * VERTICAL_PADDING + VERTICAL_OFFSET, Screen.width, VERTICAL_PADDING), texts[i], style);
            }
        }
    }
}
