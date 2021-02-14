using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugItem
{
    public virtual string Text { get; set; }
    public virtual Action<Stack<DebugPage>> OnClick { get; set; }

    public DebugItem(string text, Action<Stack<DebugPage>> onClick)
    {
        Text = text;
        OnClick = onClick;
    }

    public DebugItem(string text) : this(text, _ => { })
    {

    }
}

public class ToggleDebugItem : DebugItem
{
    public override string Text
    {
        get
        {
            if (Read())
                return base.Text + "  [x]";

            return base.Text + "  [ ]";
        }
    }

    public Func<bool> Read { get; }

    public ToggleDebugItem(string text, Action<Stack<DebugPage>> onClick, Func<bool> read) : base(text, onClick)
    {
        Read = read;
    }
}