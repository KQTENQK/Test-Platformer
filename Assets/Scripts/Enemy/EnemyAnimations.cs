using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(Animator))]

public class EnemyAnimations : MonoBehaviour
{
    private EnemyMovement _movement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement.ChangeDirection += OnChangeDirection;
    }

    private void Update()
    {
        _animator.SetBool("IsWalking", _movement.IsWalking);
    }

    private void OnChangeDirection()
    {
        _spriteRenderer.flipX = _movement.IsWalkingLeft;
    }
}
