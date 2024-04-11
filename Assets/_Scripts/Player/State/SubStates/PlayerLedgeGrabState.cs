using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLedgeGrabState : PlayerState
{
    public PlayerLedgeGrabState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
        player.DisableGravity();
        isGrabbingLedge = true;
        Subscribe();

    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.EnableGravity();
        player.StartCoroutine(StopLedgeRaysFor(.5f));
        Unsubscribe();
        isGrabbingLedge = false;

    }

    private void Subscribe()
    {
        PlayerInputManager.jumpPerformed += OnJump;
        PlayerInputManager.movePerformed += OnMove;

    }

    private void Unsubscribe()
    {
        PlayerInputManager.jumpPerformed -= OnJump;
        PlayerInputManager.movePerformed -= OnMove;

    }

    public void OnMove()
    {
        player.StartCoroutine(StopLedgeRaysFor(.5f));
        stateMachine.ChangeState(stateMachine.airState);
    }

    private void OnJump()
    {

        stateMachine.ChangeState(stateMachine.jumpState);
    }
}
