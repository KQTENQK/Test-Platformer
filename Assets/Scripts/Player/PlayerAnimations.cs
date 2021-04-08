using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerAnimations : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement.ChangeDirection += Flip;
        _playerMovement.Jumped += Jump;
    }

    private void OnEnable()
    {
        _playerMovement.BeganWalking += OnBeganWalking;
        _playerMovement.StoppedWalking += OnStopWalking;
    }

    private void OnDisable()
    {
        _playerMovement.BeganWalking -= OnBeganWalking;
        _playerMovement.StoppedWalking -= OnStopWalking;
    }

    private void OnBeganWalking()
    {
        _animator.SetBool("IsWalking", true);
    }
    private void OnStopWalking()
    {
        _animator.SetBool("IsWalking", false);
    }

    private void Flip()
    {
        _spriteRenderer.flipX = _playerMovement.IsWalkingLeft;
    }

    private void Jump()
    {
        _animator.SetTrigger("Jump");
    }
}
