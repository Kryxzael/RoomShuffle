using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[ExecuteInEditMode]
public class DebugScreenDrawer : MonoBehaviour
{
    Dictionary<string, string> Q = new Dictionary<string, string>();

    private static DebugScreenDrawer Singleton;

    private void Awake()
    {
        Singleton = this;
    }

    public static void Enable(string identifier, string text)
    {
        Singleton.Q[identifier] = text;
    }

    public static void EnableIf(string identifier, string text, bool enabled)
    {
        if (enabled)
        {
            Enable(identifier, text);
        }
        else
        {
            Disable(identifier);
        }
        
    }

    public static void Disable(string identifier)
    {
        Singleton.Q.Remove(identifier);
    }

    private void OnGUI()
    {
        const int K = 20;

        for (int i = 0; i < Q.Count; i++)
        {
            GUI.Label(new Rect(5, i * K, 9000, K), Q.ElementAt(i).Value);
        }
    }
}
