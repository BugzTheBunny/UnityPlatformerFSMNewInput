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
        IsOnWall();
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
        stateMachine.ChangeState(stateMachine.wallJumpState);
    }


    private void Slide()
    {
       player.SetVelocity(xInput, rb.velocity.y * player.wallSlideSpeed);
    }

    private void IsOnWall()
    {
        if (rb.velocity.y == 0 || !player.isWallDetected())
            stateMachine.ChangeState(stateMachine.idleState);
    }

    
}
