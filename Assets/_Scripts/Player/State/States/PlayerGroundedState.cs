using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Subscribe();
    }


    public override void Update()
    {
        base.Update();
        CheckiIfIsFalling();

    }

    public override void Exit()
    {
        base.Exit();
        Unsubscribe();

    }

    private void Subscribe()
    {
        PlayerInputManager.jumpPerformed += OnJump;
    }

    private void Unsubscribe()
    {
        PlayerInputManager.jumpPerformed -= OnJump;

    }

    private void OnJump()
    {
        if (player.isGrounded())
            player.Flip();
            stateMachine.ChangeState(player.jumpState);
    }

    private void CheckiIfIsFalling()
    { 
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
