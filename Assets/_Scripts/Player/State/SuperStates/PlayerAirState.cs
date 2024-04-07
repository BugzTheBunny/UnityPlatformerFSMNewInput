using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine , string animName) : base(_player, _stateMachine, animName)
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
        LedgeDetection();
        if (rb.velocity.y == 0)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }



    private void Move()
    {
        if (player.canMove)
            player.SetVelocity(xInput * player.airMoveSpeed, rb.velocity.y);
    }

    private void LedgeDetection()
    {
        if (player.IsLedgeDetected())
            Debug.Log("Edge Detected!");
        
    }

}
