using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public override void Enter()
    {
        base.Enter();
        //player.SetVelocity(0, 0); // Stop on Attack
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        player.animator.SetInteger("ComboCounter", comboCounter);
    }

    public override void Update()
    {
        base.Update();
        
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

}
