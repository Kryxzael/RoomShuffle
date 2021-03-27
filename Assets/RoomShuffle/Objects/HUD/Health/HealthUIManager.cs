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

    void Start()
    {
        _lastHealthLevel = Commons.PlayerProgression.HealthLevel;
        SetHeartUIElementCount(_lastHealthLevel+3);

        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2 (HeartsPerRow * HeartMargin.x * 2, 500); //Where does this number come from?
    }

    private void Update()
    {
        int playerHealth = Commons.PlayerHealth.Health;

        //If health has changed. Update health display
        if (_lastHealth > playerHealth)
        {
            //Gradual lowering
            _lastHealth--;
            SetDisplayedHealth(_lastHealth);
        }
        else if (_lastHealth < playerHealth)
        {
            //Gradual highering
            _lastHealth++;
            SetDisplayedHealth(_lastHealth);
        }
        
        //if the players healthlevel has changed
        if (_lastHealthLevel != Commons.PlayerProgression.HealthLevel)
        {
            //Update healthlevel
            _lastHealthLevel = Commons.PlayerProgression.HealthLevel;
            SetHeartUIElementCount(_lastHealthLevel+3);
            
            SetDisplayedHealth(playerHealth);
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
            if (health - HealthController.HP_PER_HEART > 0 )
            {
                //Fill heart 100%
                heart.SetHeartFillPercentage(1f);
                health -= HealthController.HP_PER_HEART;
            }
            else
            {
                //fill heart partly
                heart.SetHeartFillPercentage((float)health / (float)HealthController.HP_PER_HEART);
                health = 0;
            }
        }
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
