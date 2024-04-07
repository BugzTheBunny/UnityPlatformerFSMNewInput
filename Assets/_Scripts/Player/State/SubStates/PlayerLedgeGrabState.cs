using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeGrabState : PlayerState
{
    public PlayerLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        Subscribe();
        player.DisableGravity();

    }

    public override void Update()
    {
        base.Update();
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
        PlayerInputManager.jumpPerformed += OnJump;

    }

    private void OnJump()
    {
        player.EnableGravity();
        player.DisableLedgeRays();
        stateMachine.ChangeState(stateMachine.jumpState);
    }
}
