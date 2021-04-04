using UnityEngine;

/// <summary>
/// A pickup that grants the player a general key
/// </summary>
public class AddTimePickup : PickupScript
{
    [Tooltip("The amount of seconds to add to the timer")]
    public float AddTime;

    //The countdowntimer in UI
    private Timer _countdownTimer;

    private void Awake()
    {
        //Find countdowntimer
        _countdownTimer = Commons.CountdownTimer.GetComponent<Timer>();
    }

    public override void OnPickup()
    {
        //Add time to the count down timer
        _countdownTimer.AddTime(AddTime);
    }
}
