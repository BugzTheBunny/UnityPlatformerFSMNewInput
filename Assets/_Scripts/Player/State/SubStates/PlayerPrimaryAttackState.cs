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

    private float inertionTime = .1f;

    public override void Enter()
    {
        OverrideAnimation("player_attack_" + comboCounter); // This overrides the attack animations depending on the combo counter.
        base.Enter();
        HandleComboCounter();
        stateDuration = inertionTime;
        player.SetVelocity(player.attackMovement[comboCounter] * player.facingDirection, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateDuration < 0)
            player.SetVelocity(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(BusyFor(.1f));
        comboCounter++;
        HandleComboCounter();
        lastTimeAttacked = Time.time;
    }

    private void HandleComboCounter()
    {
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;
    }

}