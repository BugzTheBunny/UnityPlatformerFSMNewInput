using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [Header(" Movement ")]
    [SerializeField] private float _moveSpeed = 8f; public float moveSpeed { get => _moveSpeed; }
    [SerializeField] private float _jumpForce = 15; public float jumpForce { get => _jumpForce; }
    [SerializeField] private float _wallJumpForce = 5f; public float wallJumpForce { get => _wallJumpForce; }
    [Range(0,1)]
    [SerializeField] public float _wallSlideSpeed = 0.5f; public float wallSlideSpeed { get => _wallSlideSpeed; }
    [SerializeField] public bool _canWallSlide; public bool canWallSlide { get => _canWallSlide; }
    public int facingDirection { get; private set; }
    [SerializeField] private bool _canMove; public bool canMove{ get => _canMove; }
    

    [Header(" Dash ")]
    [SerializeField] private float _dashSpeed; public float dashSpeed { get => _dashSpeed; private set => _dashSpeed = value; }
    [SerializeField] private float _dashDuration; public float dashDuration{ get => _dashDuration; private set => _dashDuration = value; }
    [SerializeField] private float _dashCooldown; public float dashCooldown{ get => _dashCooldown; private set => _dashCooldown = value; }
    [HideInInspector] private int _dashDirection = 1; public int dashDirection { get => _dashDirection; private set => _dashDirection = value; }
    public bool canDash;

    [Header(" Ground Collision ")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private float groundCastDistance;


    [Header(" Wall Collision ")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCastDistance;


    #region [--- Components ---]
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }

    #endregion

    #region [--- States ---]
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallJumpState  wallJumpState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }

    #endregion


    private void Awake()
    {
        SetStates();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
        EnableMovement();
        EnableDash();
        facingDirection = 1;
    }



    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float vX, float vY)
    {
        rb.velocity = new Vector2(vX, vY);
    }
    public void EnableMovement() => _canMove = true;
    public void DisableMovement() => _canMove = false;
    public void EnableDash() => canDash = true;
    public void DisableDash() => canDash = false;

    #region OnInputEvents
    // On Move Performed
    private void OnMovePerformed()
    {
        if (canMove)
            HandleDirection();
    }

    private void HandleDirection()
    {
        SetFacingDirection();
        Flip();
    }

    public void Flip()
    {
        transform.localScale = new Vector3(1 * facingDirection, 1, 1);
        dashDirection = facingDirection == 0 ? 1 : facingDirection;
    }

    private void SetFacingDirection()
    {
        int xInput = (int)PlayerInputManager.Instance.moveVector.x;
        if (xInput != 0)
            facingDirection = xInput;
    }

    public void SetFacingDirection(int direction)
    {
        if (direction != 0)
            facingDirection = direction;
    }

    // On Dash Performed
    private void OnDashPerformed()
    {
        if (canDash)
            StartCoroutine(DashCooldownRoutine(dashCooldown));
    }

    IEnumerator DashCooldownRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        canDash = true;
    }
    #endregion

    #region Initalization
    private void SetStates()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");

    }
    
    private void OnEnable()
    {
        PlayerInputManager.movePerformed += OnMovePerformed;
        PlayerInputManager.dashPerformed += OnDashPerformed;
    }



    private void OnDisable()
    {
        PlayerInputManager.movePerformed -= OnMovePerformed;
        PlayerInputManager.dashPerformed -= OnDashPerformed;
    }

    #endregion

    #region Checks & Gizmos
    public bool isGrounded() => Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCastDistance, whatIsGround); // GroundCheck
    public bool isWallDetected() => Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCastDistance, whatIsWall); // WallCheck

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundCheckBoxSize); // GroundCheck
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCastDistance * facingDirection, transform.position.y)); // WallCheck

    }

    #endregion
}
