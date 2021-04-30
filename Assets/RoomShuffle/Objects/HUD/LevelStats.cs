using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

/// <summary>
/// Shows information about the current health and damage levels
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class LevelStats : MonoBehaviour
{
    public const string TMP_WHITE_TEXT = "<color=\"white\">{0}</color>";
    public const string TMP_RED_TEXT = "<color=\"red\">{0}</color>";
    public const string TMP_GREEN_TEXT = "<color=\"green\">{0}</color>";

    private TMP_Text _label;
    private string _unformatedText;

    private void Start()
    {
        _label = GetComponent<TMP_Text>();
        _unformatedText = _label.text;
    }

    // Update is called once per frame
    void Update()
    {
        string formatter;

        var color = TMP_WHITE_TEXT;

        if (Commons.PlayerProgression.DamageLevel > Commons.EnemyProgression.DamageLevel)
            color = TMP_GREEN_TEXT;

        else if (Commons.PlayerProgression.DamageLevel < Commons.EnemyProgression.DamageLevel && DateTime.Now.Millisecond % 500 > 250)
            color = TMP_RED_TEXT;

        formatter = string.Format(color, Commons.PlayerProgression.DamageLevel + 1);

        _label.text = string.Format(_unformatedText, formatter);
    }
}
