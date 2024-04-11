using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine , string animName) : base(_player, _stateMachine, animName)
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
        OnWallSlide();
        player.EnableLedgeRays();
        if (rb.velocity.y < 0)
            LedgeDetection();
        Move();
        if (player.IsGrounded() && rb.velocity.y == 0)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        Unsubscribe();
    }

    private void Subscribe()
    {
        PlayerInputManager.attackPerformed += OnAttack;
    }


    private void Unsubscribe()
    {
        PlayerInputManager.attackPerformed -= OnAttack;

    }

    private void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.airAttackState);
    }

    private void Move()
    {
        if (player.canMove)
            player.SetVelocity(xInput * player.airMoveSpeed, rb.velocity.y);
    }

    private void OnWallSlide()
    {
        if (player.IsWallDetected() && !player.IsGrounded() && rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.wallSlideState);
        }
    }


}
