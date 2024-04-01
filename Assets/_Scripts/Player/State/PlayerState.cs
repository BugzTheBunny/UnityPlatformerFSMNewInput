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
    protected float stateDuration; // Certein States have "time to be alive", like Dash.
    protected float xInput;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        PlayerInputManager.Instance.actions.Player.Dash.performed += OnDashPerformed;
        if (_animBoolName != "")
            player.animator.SetBool(_animBoolName, true);
        rb = player.rb;
    }

    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        SetAxis();
        OnWallSlide();
    }
    public virtual void Exit()
    {
        if (_animBoolName != "")
            player.animator.SetBool(_animBoolName, false); // Ends animation on change.
        PlayerInputManager.Instance.actions.Player.Dash.performed -= OnDashPerformed;

    }

    private void OnDashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (player.canDash)
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    private void OnWallSlide()
    {
        if (player.isWallDetected() && !player.isGrounded() && rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }


    private void SetAxis()
    {
        SetX();
        SetY();
    }

    private void SetX()
    {
        if (player.canMove)
        {
            xInput = PlayerInputManager.Instance.moveVector.x;
        }
    }

    /// <summary>
    /// There is no Y Axis movement here, but we do set the Y value for the usage of the animation.
    /// </summary>
    private void SetY()
    {
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }


}