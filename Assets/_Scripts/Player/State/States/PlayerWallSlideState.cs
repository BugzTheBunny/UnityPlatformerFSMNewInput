using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    int _wallDirection;

    public override void Enter()
    {
        base.Enter();
        PlayerInputManager.jumpPerformed += OnJump;
        _wallDirection = player.facingDirection;

    }


    public override void Update()
    {
        base.Update();
        Slide();
        CheckIfChangeState();
    }

    

    public override void Exit()
    {
        base.Exit();
        PlayerInputManager.jumpPerformed -= OnJump;

    }

    private void OnJump()
    {
        player.SetFacingDirection(-player.facingDirection);
        player.Flip();
        stateMachine.ChangeState(player.wallJumpState);
    }


    private void Slide()
    {
       player.SetVelocity(xInput, rb.velocity.y * player.wallSlideSpeed);

    }

    /// <summary>
    /// If not movement on Y || wall not detected || player not sticking to wall via input.
    /// </summary>
    private void CheckIfChangeState()
    {
        if (rb.velocity.y == 0 || !player.isWallDetected())
        {
            Debug.Log("Left Wall Slide");
            stateMachine.ChangeState(player.idleState);
        }
    }

    
}
