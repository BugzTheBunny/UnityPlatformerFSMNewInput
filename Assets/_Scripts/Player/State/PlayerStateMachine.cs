using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    private Player player = Player.instance;

    #region [--- States ---]
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    #endregion

    public void Initialize()
    {
        CreateStateMachine();
        currentState = idleState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit(); // Ends current state
        currentState = newState; // Sets new state
        currentState.Enter(); // Enters / Starts the new state
    }

    public void CreateStateMachine()
    {
        idleState = new PlayerIdleState(player, this, "Idle");
        moveState = new PlayerMoveState(player, this, "Move");
        jumpState = new PlayerJumpState(player, this, "Jump");
        airState = new PlayerAirState(player, this, "Jump");
        wallSlideState = new PlayerWallSlideState(player, this, "WallSlide");
        wallJumpState = new PlayerWallJumpState(player, this, "WallJump");
        dashState = new PlayerDashState(player, this, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(player, this, "Attack");

    }

}