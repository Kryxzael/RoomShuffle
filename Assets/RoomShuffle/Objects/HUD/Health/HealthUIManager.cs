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
        AddHeart(30);
        SetHealth(560);
        RemoveHeart();

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

    public void AddHeart(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            int row = (_heartList.Count / HeartsPerRow) + 1;
            int column = (_heartList.Count % HeartsPerRow) + 1;
            
            GameObject newHeart = Instantiate(HeartPrefab, new Vector3(HeartDistanceX * column, HeartDistanceY * -row, 0) + transform.position, Quaternion.identity, transform);

            _heartList.Add(new Tuple<GameObject, Heart>(newHeart, newHeart.GetComponent<Heart>()));
        }
    }

    public void RemoveHeart(int numberOfHearts = 1)
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Tuple<GameObject, Heart> heartTuple = _heartList.Last();
            Destroy(heartTuple.Item1);
            _heartList.Remove(heartTuple);
        }
        
    }
}
