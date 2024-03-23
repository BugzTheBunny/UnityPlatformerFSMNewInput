using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{

    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    private string animBoolName;

    protected float stateTimer;
    public float dashUsageTimer;


    protected float xInput;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        PlayerInputManager.Instance.actions.Player.Dash.performed += OnDashPerformed;
        player.animator.SetBool(animBoolName, true);
        rb = player.rb;
    }

    private void OnDashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (dashUsageTimer < 0)
        {
            dashUsageTimer = player.dashCooldown;
            stateMachine.ChangeState(player.dashState);
        }
    }

    public virtual void Update()
    {
        dashUsageTimer -= Time.deltaTime;
        stateTimer -= Time.deltaTime;
        SetAxis();
    }


    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }

    private void SetAxis()
    {
        xInput = PlayerInputManager.Instance.moveVector.x;
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }


}