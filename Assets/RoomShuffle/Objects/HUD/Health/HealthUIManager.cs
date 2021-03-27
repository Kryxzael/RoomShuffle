using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the Health (Heart-like) user interface
/// </summary>
public class HealthUIManager : MonoBehaviour
{
    [Tooltip("The heart that represents 100 health")]
    public Heart HeartPrefab;

    [Tooltip("The distance between each heart")]
    public Vector2 HeartMargin = new Vector2(30f, 25f);
    
    [Tooltip("The number of hearts per row")]
    public int HeartsPerRow = 5;

    //Holds the heart objects 
    private Stack<Heart> _heartStack = new Stack<Heart>();

    //The owner's last known health
    private int _lastHealth;
    
    //The owner's last known health
    private int _lastHealthLevel;

    //Is the health changing animation currently running?
    private bool _healthChangeAnimationRunning;

    void Start()
    {
        _lastHealthLevel = Commons.PlayerProgression.HealthLevel;
        SetHeartUIElementCount(_lastHealthLevel+3);

        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.sizeDelta = new Vector2 (HeartsPerRow * HeartMargin.x * 2, 500); //Where does this number come from?
    }

    private void Update()
    {
        int playerHealth = Commons.PlayerHealth.Health;
        
        //if the players healthlevel has changed
        if (_lastHealthLevel != Commons.PlayerProgression.HealthLevel)
        {
            //Update healthlevel
            _lastHealthLevel = Commons.PlayerProgression.HealthLevel;
            SetHeartUIElementCount(Commons.PlayerProgression.GetMaximumHealth() / HealthController.HP_PER_HEART);

        }
        
        //The player's health has changed
        if (_lastHealth != playerHealth)
        {
            int animationInitialHealth = _lastHealth;
            _lastHealth = playerHealth;
            AnimateHeartsToCurrentHealth(animationInitialHealth);
        }

    }

    /// <summary>
    /// Starts or continues an animation of loosing or gaining health from the provided starting health to the current health value
    /// </summary>
    /// <param name="initialHealth"></param>
    public void AnimateHeartsToCurrentHealth(int initialHealth)
    {
        //Increase size of affected hearts
        {
            int firstAffectedHeart = Math.Min(initialHealth, _lastHealth) / HealthController.HP_PER_HEART;
            int lastAffectedHeart = Math.Max(initialHealth, _lastHealth) / HealthController.HP_PER_HEART;

            int index = 0;


            foreach (Heart i in _heartStack.Reverse())
            {
                if (index >= lastAffectedHeart)
                    break;

                if (index >= firstAffectedHeart)
                    i.transform.localScale = Vector3.one * 1.5f;

                index++;
            }
        }
            

        if (!_healthChangeAnimationRunning)
        {
            _healthChangeAnimationRunning = true;
            StartCoroutine(CoAnimateHealthChange(initialHealth));
        }
    }

    /// <summary>
    /// Sets the amount of health to display
    /// </summary>
    /// <param name="health"></param>
    public void SetDisplayedHealth(int health)
    {
        //for each heart starting from the beginning
        foreach (Heart heart in _heartStack.Reverse())
        {
            //If heart should get maxed out
            if (health - HealthController.HP_PER_HEART > 0)
            {
                //Fill heart 100%
                heart.SetHeartFillPercentage(1f);
                health -= HealthController.HP_PER_HEART;
            }
            else
            {
                //fill heart partly
                heart.SetHeartFillPercentage((float)health / HealthController.HP_PER_HEART);
                health = 0;
            }
        }
    }

    /// <summary>
    /// Animates a health change from the provided health to the player's current health
    /// </summary>
    /// <param name="initialHealth"></param>
    /// <returns></returns>
    private IEnumerator CoAnimateHealthChange(int initialHealth)
    {
        //Keeps track of the health that is _actualy_ being displayed at this time
        int currentlyDisplayedHealth = initialHealth;

        //Wait a bit, for dramatic effect
        yield return new WaitForSecondsRealtime(0.75f);

        //As long as the displayed health doesn't match the player's actual health, we keep the animation going
        while (currentlyDisplayedHealth != _lastHealth)
        {
            //Calculate the difference between the displayed health and the player's actual health
            int difference = Math.Abs(currentlyDisplayedHealth - _lastHealth);

            //How much HP to animate per unit of time
            int changeAmount = 10;

            //If we are more than one heart from our target, we can move a little faster
            if (difference > HealthController.HP_PER_HEART)
                changeAmount = 20;

            //If we are less (or equal for effect) than `changeAmount` HP away from our target, reduce the speed to 1 so we don't overshoot
            if (difference <= changeAmount)
                changeAmount = 1;

            //The health is moving down, inverse the change
            if (currentlyDisplayedHealth > _lastHealth)
                changeAmount *= -1;

            //Apply the change and update the display
            currentlyDisplayedHealth += changeAmount;
            SetDisplayedHealth(currentlyDisplayedHealth);

            yield return new WaitForSecondsRealtime(0.05f);
        }

        //Reset size of hearts
        foreach (Heart i in _heartStack)
            i.transform.localScale = Vector3.one;

        //Stop animation
        _healthChangeAnimationRunning = false;
    }

    /// <summary>
    /// Adds a heart(s) to the player. Hearts gets added to the last index
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void CreateHeartUIElement(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            int row = (_heartStack.Count / HeartsPerRow) + 1;
            int column = (_heartStack.Count % HeartsPerRow) + 1;
            
            Heart newHeart = Instantiate(
                original: HeartPrefab, 
                position: new Vector3(HeartMargin.x * column, HeartMargin.y * -row, 0) + transform.position, 
                rotation: Quaternion.identity, 
                parent: transform
            );

            _heartStack.Push(newHeart);
            newHeart.SetHeartFillPercentage(0f);
        }
    }

    /// <summary>
    /// Destroys the last heart(s) of the player. This is the heart(s) that gets filled last
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void RemoveHeartUIElement(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Heart heartTuple = _heartStack.Pop();
            Destroy(heartTuple);
        }
    }

    /// <summary>
    /// Sets the number of hearts for the player. Add/removes last hearts. Doesn't add health
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void SetHeartUIElementCount(int desiredNumberOfHearts)
    {
        int currentNumberOfHearts = transform.GetChildren().Count();
        int difference = desiredNumberOfHearts - currentNumberOfHearts;

        if (difference >= 0)
        {
            CreateHeartUIElement(difference);
        } 
        else
        {
            RemoveHeartUIElement(-difference);
        }
    }
}
