using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class HealthUIManager : MonoBehaviour
{
    
    [Tooltip("The heart that represents health")]
    public GameObject HeartPrefab;
    
    [Tooltip("The distance between each heart horizontally")]
    public float HeartDistanceX;
    
    [Tooltip("The distance between each heart vertically")]
    public float HeartDistanceY;
    
    [Tooltip("The number of hearts per row")]
    public int HeartsPerRow;

    private List<Tuple<GameObject, Heart>> _heartList;
    private int _lastHealth;
    void Start()
    {
        _heartList = new List<Tuple<GameObject, Heart>>();
        SetHearts(3);

        RectTransform rt = transform.GetComponent (typeof (RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2 (HeartsPerRow * HeartDistanceX * 2, 500);
    }

    private void Update()
    {
        if (_lastHealth == Commons.PlayerHealth.Health)
            return;

        _lastHealth = Commons.PlayerHealth.Health;
        SetHealth(_lastHealth);
    }

    /// <summary>
    /// Displays the health of the player
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        foreach (Tuple<GameObject, Heart> heart in _heartList)
        {
            if (health - 100 >= 0)
            {
                heart.Item2.setHealth(1f);
                health -= 100;
            }
            else
            {
                heart.Item2.setHealth(health/100f);
                health = 0;
            }
        }
    }

    /// <summary>
    /// Adds a heart(s) to the player. Hearts gets added to the last index
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void AddHeart(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            int row = (_heartList.Count / HeartsPerRow) + 1;
            int column = (_heartList.Count % HeartsPerRow) + 1;
            
            GameObject newHeart = Instantiate(
                original: HeartPrefab, 
                position: new Vector3(HeartDistanceX * column, HeartDistanceY * -row, 0) + transform.position, 
                rotation: Quaternion.identity, 
                parent: transform);

            _heartList.Add(new Tuple<GameObject, Heart>(newHeart, newHeart.GetComponent<Heart>()));
        }
    }

    /// <summary>
    /// Destroys the last heart(s) of the player. This is the heart(s) that gets filled last
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void RemoveHeart(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Tuple<GameObject, Heart> heartTuple = _heartList.Last();
            Destroy(heartTuple.Item1);
            _heartList.Remove(heartTuple);
        }
    }

    /// <summary>
    /// Sets the number of hearts for the player. Add/removes last hearts. Doesn't add health
    /// </summary>
    /// <param name="numberOfHearts"></param>
    public void SetHearts(int desiredNumberOfHearts)
    {
        int currentNumberOfHearts = 0;
        foreach (var child in transform)
        {
            currentNumberOfHearts++;
        }
        int difference = desiredNumberOfHearts - currentNumberOfHearts;

        if (difference >= 0)
        {
            AddHeart(difference);
        } 
        else
        {
            RemoveHeart(-difference);
        }
    }
}
