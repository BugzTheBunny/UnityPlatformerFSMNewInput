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

    protected float stateDuration; // Certein States have "time to be alive", like Dash.
    
    [Header ("Dash")]
    public float dashUsageTimer;
    
    protected float xInput;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        this._animBoolName = animBoolName;
    }

    /// <summary>
    /// This method is executed when entering a new state.
    /// </summary>
    public virtual void Enter()
    {
        PlayerInputManager.Instance.actions.Player.Dash.performed += OnDashPerformed;
        player.animator.SetBool(_animBoolName, true);
        rb = player.rb;
    }


    /// <summary>
    /// This method is executed on-update (each state has it's own)
    /// </summary>
    public virtual void Update()
    {
        stateDuration -= Time.deltaTime;
        SetAxis();
    }

    /// <summary>
    /// This method is executed when leaving a state 
    /// </summary>
    public virtual void Exit()
    {
        player.animator.SetBool(_animBoolName, false); // Ends animation on change.
        PlayerInputManager.Instance.actions.Player.Dash.performed -= OnDashPerformed;

    }

    /// <summary>
    /// Activates when pressing dash button.
    /// </summary>
    private void OnDashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (player.canDash)
        {
            dashUsageTimer = player.dashCooldown;
            stateMachine.ChangeState(player.dashState);
        }
    }


    private void SetAxis()
    {
        SetX();
        SetY();
    }

    private void SetX()
    {
        xInput = PlayerInputManager.Instance.moveVector.x;
    }

    /// <summary>
    /// There is no Y Axis movement here, but we do set the Y value for the usage of the animation.
    /// </summary>
    private void SetY()
    {

        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }


}