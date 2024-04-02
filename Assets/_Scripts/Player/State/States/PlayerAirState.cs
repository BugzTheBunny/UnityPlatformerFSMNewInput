using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        Move();
        if (rb.velocity.y == 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

    }

    protected override void Subscribe()
    {
        PlayerInputManager.dashPerformed += OnDashPerformed;
    }

    protected override void Unsubscribe()
    {
        PlayerInputManager.dashPerformed -= OnDashPerformed;
    }

    private void Move()
    {
        if (player.canMove)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        }
    }
}
