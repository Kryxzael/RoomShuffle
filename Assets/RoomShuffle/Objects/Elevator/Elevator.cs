using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    public float Speed;
    
    //The elevators current goal
    private Vector2 _endPoint;
    
    //the elevators current start position
    private Vector2 _startPoint;
    
    //The elevators elevator manager
    private ElevatorManager _elevatorManager;
    private float _journeyPercentage = 0;
    private EndOfLineOption _endOfLineOption;
    private List<Vector2> _checkPointList = new List<Vector2>();
    private int _stage = 0;
    private int _maxStage;
    private float _journeyDistance;
    void Start()
    {
        _elevatorManager = transform.GetComponentInParent<ElevatorManager>();
        _elevatorManager.GetCheckpointList().ForEach(x => _checkPointList.Add(x));
        _maxStage = _checkPointList.Count - 1;
        _endOfLineOption = _elevatorManager.EOLOption;
            
        _startPoint = _checkPointList[0];
        _endPoint = _checkPointList[1];
        _journeyDistance = Vector2.Distance(_startPoint, _endPoint);
    }
    
    void Update()
    {
        _journeyPercentage += (Time.deltaTime * Speed) / _journeyDistance;
        transform.position = Vector2.Lerp(_startPoint, _endPoint, _journeyPercentage);
        
        //The elevator has reached its goal
        if (_journeyPercentage >= 1f)
        {
            if (_stage == -1)
            {
                _elevatorManager.CloseLoop();
            }

            _stage++;
            _journeyPercentage = 0;
            
            //If there is still more checkpoints
            if (_stage < _maxStage)
            {
                _startPoint = _checkPointList[_stage];
                _endPoint = _checkPointList[_stage+1];
            }
            //End of line
            else
            {
                //Return journey
                switch (_endOfLineOption)
                {
                    case EndOfLineOption.Destroy: 
                        Destroy(gameObject);
                        return;
                        break;
                    case EndOfLineOption.Loop: 
                        _stage = -1;
                        _startPoint = _checkPointList.Last();
                        _endPoint = _checkPointList.First();
                        break;
                    case EndOfLineOption.Return: 
                        _stage = 0;
                        _checkPointList.Reverse();
                        _startPoint = _checkPointList[_stage];
                        _endPoint = _checkPointList[_stage+1];
                        break;
                }
            }

            _journeyDistance = Vector2.Distance(_startPoint, _endPoint);
        }
        
    }
}
