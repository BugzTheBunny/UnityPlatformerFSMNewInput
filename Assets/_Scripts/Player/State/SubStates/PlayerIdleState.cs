using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine,  string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {

        base.Enter();
        if (player.IsGrounded())
            player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0 && player.canMove)
            stateMachine.ChangeState(stateMachine.moveState);
    }
}