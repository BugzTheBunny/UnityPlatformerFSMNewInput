using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
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
        PlayerInputManager.attackPerformed += OnAttack;
    }


    private void Unsubscribe()
    {
        PlayerInputManager.jumpPerformed -= OnJump;
        PlayerInputManager.attackPerformed -= OnAttack;

    }

    private void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.primaryAttackState);
    }


    private void OnJump()
    {
        if (player.IsGrounded())
            player.Flip();
            stateMachine.ChangeState(stateMachine.jumpState);
    }

    private void CheckiIfIsFalling()
    { 
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(stateMachine.airState);
    }
}
