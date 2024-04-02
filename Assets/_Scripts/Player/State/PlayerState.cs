using System;
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
    protected float xInput;
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        Subscribe();

        player.animator.SetBool(_animBoolName, true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        SetYVelocityAnimatorParam();
        if (player.canWallSlide)
            OnWallSlide();
    }

    public virtual void Exit()
    {
        player.animator.SetBool(_animBoolName, false);

        Unsubscribe();
    }

    /// <summary>
    /// Used to subscribe to events, in this Base subscribe, we sub to movePerformed and moveCanceled to set the xInput for all the states.
    /// </summary>
    protected virtual void Subscribe()
    {
        PlayerInputManager.movePerformed += OnMove;
        PlayerInputManager.moveCanceled += OnMoveCanceled;
    }



    /// <summary>
    /// Unsubscribe from the events.
    /// </summary>
    protected virtual void Unsubscribe()
    {
        PlayerInputManager.movePerformed -= OnMove;
        PlayerInputManager.moveCanceled -= OnMoveCanceled;

    }

    protected void OnDashPerformed()
    {
        if (player.canDash)
            stateMachine.ChangeState(player.dashState);
    }

    protected virtual void OnMove()
    {
        xInput = PlayerInputManager.Instance.moveVector.x;
    }

    protected void OnMoveCanceled()
    {
        throw new NotImplementedException();
    }


    private void OnWallSlide()
    {
        if (player.isWallDetected() && !player.isGrounded() && rb.velocity.y <= 0)
            stateMachine.ChangeState(player.wallSlideState);
    }

    private void SetYVelocityAnimatorParam()
    {
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }

}