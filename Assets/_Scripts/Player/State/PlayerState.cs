using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{

    [Header(" Components ")]
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    private string _animBoolName;


    [Header(" Settings ")]
    protected float stateDuration;

    [Header(" State / Triggers ")]
    protected bool canAttack = true;
    protected bool triggerCalled;
    protected bool isBusy = false;
    protected float xInput = 0;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        PlayerInputManager.dashPerformed += OnDashPerformed;
        player.animator.SetBool(_animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        SetX();
        SetY();
        if (player.canWallSlide)
            OnWallSlide();

    }

    public virtual void Exit()
    {
        player.animator.SetBool(_animBoolName, false); 
        PlayerInputManager.dashPerformed -= OnDashPerformed;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    #region Shared across all states
    private void OnDashPerformed()
    {
        if (player.canDash)
            stateMachine.ChangeState(stateMachine.dashState);
    }

    private void OnWallSlide()
    {
        if (player.IsWallDetected() && !player.IsGrounded() && rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.wallSlideState);
        }
    }

    private void SetX()
    {
        xInput = Player.instance.moveDirection;
    }

    private void SetY()
    {
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }

    protected IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        player.DisableMovement();
        yield return new WaitForSeconds(_seconds);
        player.EnableMovement();
        isBusy = false;
    }
    #endregion



}