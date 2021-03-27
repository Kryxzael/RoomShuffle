using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float Speed;
    
    private Vector2 _endPoint;
    private ElevatorManager _elevatorManager;
    private Vector2 _startPoint;
    private float _journeyPercentage = 0;
    private bool _shouldReturn;
    void Start()
    {
        _elevatorManager = transform.GetComponentInParent<ElevatorManager>();
        _endPoint = _elevatorManager.EndPoint;
        _startPoint = _elevatorManager.StartPoint;
        _shouldReturn = _elevatorManager.ShouldReturn;
    }
    
    void Update()
    {
        _journeyPercentage += Time.deltaTime * Speed;

        if (_journeyPercentage >= 1f)
        {
            if (_shouldReturn)
            {
                Vector2 endPointPlaceHolder = _endPoint;
                _endPoint = _startPoint;
                _startPoint = endPointPlaceHolder;
                _journeyPercentage = 0;

            }
            else
            {
                Destroy(gameObject);
                            return;
            }
        }

        transform.position = Vector2.Lerp(_startPoint, _endPoint, _journeyPercentage);
    }
}
