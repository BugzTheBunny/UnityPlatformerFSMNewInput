using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine,  string animName) : base(_player, _stateMachine, animName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }


    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(stateMachine.airState);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
