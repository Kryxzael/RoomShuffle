using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPage : IEnumerable
{
    public string Header { get; }
    public List<DebugItem> Items { get; } = new List<DebugItem>();

    public int SelectedIndex { get; set; }

    public DebugPage(string header)
    {
        Header = header;
    }

    public void Add(DebugItem item)
    {
        Items.Add(item);
    }

    public IEnumerator GetEnumerator()
    {
        return Items.GetEnumerator();
    }
}
