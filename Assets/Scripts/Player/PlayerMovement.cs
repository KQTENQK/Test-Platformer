using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _minGroundNormalY;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _minMoveDistance;
    [SerializeField] private float _shellRadius;
    [SerializeField] private float _speed = 5;

    private float _distanceX;
    private float _distanceY;
    private Vector2 _moveDistanceX;
    private Vector2 _moveDistanceY;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private Rigidbody2D _rigidbody2D;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);

    public event Action Jumped;
    public event Action ChangeDirection;
    public event Action BeganWalking;
    public event Action StoppedWalking;

    public bool IsWalkingLeft { get; private set; }
    public bool Grounded { get; private set; }

    private void OnEnable()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Jumped += OnJumped;
    }

    private void OnDisable()
    {
        Jumped -= OnJumped;
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        _targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (_targetVelocity.x < 0)
        {
            BeganWalking?.Invoke();
            IsWalkingLeft = true;
            ChangeDirection?.Invoke();
        }
        else if (_targetVelocity.x == 0)
        {
            StoppedWalking?.Invoke();
        }
        else
        {
            BeganWalking?.Invoke();
            IsWalkingLeft = false;
            ChangeDirection?.Invoke();
        }

        if (Input.GetKey(KeyCode.Space) && Grounded)
            Jumped?.Invoke();
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x * _speed;

        Grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 distanceAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        _moveDistanceX = distanceAlongGround * deltaPosition.x;

        _distanceX = GetDistance(_moveDistanceX, false);

        _moveDistanceY = Vector2.up * deltaPosition.y;

        _distanceY = GetDistance(_moveDistanceY, true);

        _rigidbody2D.position = _rigidbody2D.position + _moveDistanceX.normalized * _distanceX;
        _rigidbody2D.position = _rigidbody2D.position + _moveDistanceY.normalized * _distanceY;
    }

    private float GetDistance(Vector2 moveDistance, bool isYMovement)
    {
        float distance = moveDistance.magnitude;

        if (distance > _minMoveDistance)
        {
            int hits = _rigidbody2D.Cast(moveDistance, _contactFilter, _hitBuffer, distance + _shellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < hits; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    Grounded = true;

                    if (isYMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                    float projection = Vector2.Dot(_velocity, currentNormal);

                    if (projection < 0)
                    {
                        _velocity = _velocity - projection * currentNormal;
                    }

                    float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
        }

        return distance;
    }

    private void OnJumped()
    {
        _velocity.y = 20;
    }
}
