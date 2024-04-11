using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWallJumpState : PlayerAirState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Flip();
        player.SetVelocity(player.wallJumpForce * -player.facingDirection, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
