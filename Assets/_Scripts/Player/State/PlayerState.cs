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

    [Header(" Settings ")]
    protected float stateDuration;
    private string _animName;
    protected int animIndex = -1;

    [Header(" State / Triggers ")]
    protected bool canAttack = true;
    protected bool triggerCalled;
    protected bool isGrabbingLedge;
    protected bool isBusy = false;
    protected float xInput = 0;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animName = animName;
    }

    public virtual void Enter()
    {
        PlayerInputManager.dashPerformed += OnDashPerformed;
        PlayAnimation();
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        SetX();
        SetY();
    }

    public virtual void Exit()
    {
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

    public IEnumerator StopLedgeRaysFor(float _seconds)
    {
        player.DisableLedgeRays();
        yield return new WaitForSeconds(_seconds);
        player.EnableLedgeRays();

    }


    #endregion

    private void PlayAnimation()
    {
        player.animator.Play(_animName);
    }

    protected void OverrideAnimation(string animationName)
    {
        this._animName = animationName;
    }

    protected void LedgeDetection()
    {
        if (player.IsLedgeDetected())
            stateMachine.ChangeState(stateMachine.ledgeGrabState);

    }


}