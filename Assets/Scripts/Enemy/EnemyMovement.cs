using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyMovement : MonoBehaviour
{
    public event Action ChangeDirection;

    public bool IsWalking { get; private set; }
    public bool IsWalkingLeft { get; private set; }

    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToBeginWalk = 2;
    private float _elapsedTimeToBeginWalk;

    private Transform[] _points;
    private int _currentPointIndex;

    private void Start()
    {
        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    private void Update()
    {
        if (_elapsedTimeToBeginWalk >= _timeToBeginWalk)
        {
            Transform target = _points[_currentPointIndex];
            IsWalkingLeft = (transform.position.x - target.position.x) > 0 ? true : false;
            transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            IsWalking = true;

            if (transform.position == target.position)
            {
                IsWalking = false;
                _elapsedTimeToBeginWalk = 0;
                _currentPointIndex++;
                ChangeDirection?.Invoke();

                if (_currentPointIndex >= _points.Length)
                    _currentPointIndex = 0;
            }
        }
        else
        {
            _elapsedTimeToBeginWalk += Time.deltaTime;
        }
    }
}
