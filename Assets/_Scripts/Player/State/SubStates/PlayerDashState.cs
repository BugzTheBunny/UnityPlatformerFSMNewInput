using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StartCoroutine(DashCooldownRoutine(player.dashCooldown));
        stateDuration = player.dashDuration;
        player.DisableDash();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);
        if (stateDuration < 0)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }

    public IEnumerator DashCooldownRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        player.EnableDash();
    }

}
