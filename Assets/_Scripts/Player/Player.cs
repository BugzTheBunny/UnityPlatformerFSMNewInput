using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header(" Movement ")]
    [SerializeField] public float moveSpeed = 8f;
    [SerializeField] public float jumpForce = 15;
    public int facingDirection { get; private set; }


    [Header(" Dash ")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public float dashCooldown;
    [HideInInspector] public int dashDirection = 1;
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

    #endregion


    #region [--- States ---]
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }

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
        canDash = true;
    }



    private void Update()
    {
        stateMachine.currentState.Update();
        Debug.Log("Current State : " + stateMachine.currentState);

    }

    public void SetVelocity(float vX, float vY)
    {
        rb.velocity = new Vector2(vX, vY);
    }


    #region OnInputEvents
    // On Move Performed
    private void OnMovePerformed()
    {
        HandleDirection();
    }

    private void HandleDirection()
    {
        facingDirection = (int)PlayerInputManager.Instance.moveVector.x;
        if (facingDirection != 0)
            Flip();
    }

    private void Flip()
    {
        transform.localScale = new Vector3(1 * facingDirection, 1, 1);
        dashDirection = facingDirection == 0 ? 1 : facingDirection;
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
        dashState = new PlayerDashState(this, stateMachine, "Dash");
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
    public bool isWallDetected() => Physics2D.Raycast(transform.position, Vector2.right, wallCastDistance, whatIsWall); // WallCheck

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundCheckBoxSize); // GroundCheck
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCastDistance * dashDirection, transform.position.y)); // WallCheck

    }

    #endregion
}
