using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

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

    protected override void Subscribe()
    {
        base.Subscribe();
        PlayerInputManager.jumpPerformed += OnJump;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        PlayerInputManager.jumpPerformed -= OnJump;
    }
}
