using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }



    private void Move()
    {
        if (player.canMove)
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
            if (xInput == 0)
                stateMachine.ChangeState(stateMachine.idleState);
    }
}