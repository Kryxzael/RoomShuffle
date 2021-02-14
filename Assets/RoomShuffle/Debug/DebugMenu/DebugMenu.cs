using Assets.RoomShuffle.Debug.DebugMenu;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private Stack<DebugPage> NavigationStack = new Stack<DebugPage>();

    public DebugPage CurrentPage
    {
        get => NavigationStack.Peek();
    }

    private void Awake()
    {
        NavigationStack.Push(DebugPages.Home); 
    }

    private bool lastFrameHeldButton;

    private void Update()
    {
        if (Input.GetAxisRaw("DebugVertical") == -1)
        {
            if (lastFrameHeldButton)
                return;

            CurrentPage.SelectedIndex = Mathf.Min(CurrentPage.SelectedIndex + 1, CurrentPage.Items.Count - 1);
        }

        else if (Input.GetAxisRaw("DebugVertical") == 1)
        {
            if (lastFrameHeldButton)
                return;

            CurrentPage.SelectedIndex = Mathf.Max(CurrentPage.SelectedIndex -1, 0);
        }

        else if (Input.GetAxisRaw("DebugHorizontal") == 1)
        {
            if (lastFrameHeldButton)
                return;

            CurrentPage.Items[CurrentPage.SelectedIndex].OnClick(NavigationStack);
        }

        else if (Input.GetAxisRaw("DebugHorizontal") == -1)
        {
            if (lastFrameHeldButton)
                return;

            if (NavigationStack.Count > 1)
                NavigationStack.Pop();
        }
        else
        {
            lastFrameHeldButton = false;
            return;
        }

        lastFrameHeldButton = true;
    }

    private void OnGUI()
    {
        List<string> toDraw = new List<string>();
        toDraw.Add("== DEBUG MODE ==");
        toDraw.Add(CurrentPage.Header);
        toDraw.Add("");

        for (int i = 0; i < CurrentPage.Items.Count; i++)
        {
            string text = CurrentPage.Items[i].Text;

            if (CurrentPage.SelectedIndex == i)
                text = ">> " + text + " <<";

            toDraw.Add(text);
        }

        DrawList(toDraw.ToArray());
    }

    private void DrawList(string[] texts)
    {
        GUI.color = Color.white;

        for (int i = 0; i < texts.Length; i++)
        {
            const float VERTICAL_PADDING = 20f;
            GUI.Label(new Rect(0f, i * VERTICAL_PADDING, Screen.width, VERTICAL_PADDING), texts[i], new GUIStyle() { alignment = TextAnchor.UpperRight });
        }
    }
}
