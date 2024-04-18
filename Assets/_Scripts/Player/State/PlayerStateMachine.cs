using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public Animation currentStateAnimation { get; private set; }
    private Player player;

    #region [--- States ---]
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerAirAttackState airAttackState { get; private set; }
    public PlayerLedgeGrabState ledgeGrabState{ get; private set; }
    #endregion

    public void Initialize()
    {
        SetOnInit();
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
        idleState = new PlayerIdleState(player, this, "player_idle");
        moveState = new PlayerMoveState(player, this, "player_move");
        
        jumpState = new PlayerJumpState(player, this, "air_state");
        airState = new PlayerAirState(player, this, "air_state");
        wallJumpState = new PlayerWallJumpState(player, this, "air_state");
        airAttackState = new PlayerAirAttackState(player, this, "player_air_attack");

        wallSlideState = new PlayerWallSlideState(player, this, "player_wallslide");
        ledgeGrabState = new PlayerLedgeGrabState(player, this, "player_wallslide");

        dashState = new PlayerDashState(player, this, "player_dash");

        primaryAttackState = new PlayerPrimaryAttackState(player, this, "player_attack");


    }

    private void SetOnInit()
    {
        player = Player.instance;
    }

}