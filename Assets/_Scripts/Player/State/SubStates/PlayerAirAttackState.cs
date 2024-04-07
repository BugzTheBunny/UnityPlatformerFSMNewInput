using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : PlayerAirState
{
    public PlayerAirAttackState(Player _player, PlayerStateMachine _stateMachine, string animName) : base(_player, _stateMachine, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 10);
        stateDuration = .2f;
    }

    public override void Update()
    {
        base.Update();
        if (stateDuration < 0)
            stateMachine.ChangeState(stateMachine.airState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
