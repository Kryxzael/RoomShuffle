using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

//TODO: This script is (probably) highly incompatible with the timer script. Shome mutual exclusivity should be ensured here

/// <summary>
/// Displays the amount of enemies remaining in an eradication room on a GUI label
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class EradicationProgress : MonoBehaviour
{
    public bool UseOnlyOnSpeedRunMode = false;
    
    private TextMeshProUGUI _label;

    [Tooltip("The formatting string used to display the text")]
    public string EradicationTextFormat = "Enemies Left: {0}";

    private void Awake()
    {
        _label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {

        if (UseOnlyOnSpeedRunMode && !Commons.SpeedRunMode)
            return;

        if (!UseOnlyOnSpeedRunMode && Commons.SpeedRunMode)
            return;

        if (Commons.CurrentRoomEffects.HasFlag(RoomEffects.Timer))
            return;

        if (Commons.RoomGenerator.CurrentRoomConfig.Class != RoomClass.Eradication)
        {
            _label.text = "";
            return;
        }

        _label.text = string.Format(EradicationTextFormat, arg0: FindObjectsOfType<EnemyBase>().Length);
    }
}
