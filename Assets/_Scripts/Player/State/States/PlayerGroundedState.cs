using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }


    public override void Update()
    {
        base.Update();
        CheckiIfIsFalling();

    }

    public override void Exit()
    {
        base.Exit();
    }


    protected override void Subscribe()
    {
        PlayerInputManager.jumpPerformed += OnJump;
        PlayerInputManager.dashPerformed += OnDashPerformed;
    }

    protected override void Unsubscribe()
    {
        PlayerInputManager.jumpPerformed -= OnJump;
        PlayerInputManager.dashPerformed -= OnDashPerformed;
    }

    private void OnJump()
    {
        stateMachine.ChangeState(player.jumpState);
    }

    private void CheckiIfIsFalling()
    {
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
