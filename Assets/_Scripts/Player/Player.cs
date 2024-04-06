using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    

    [Header(" Movement ")]
    [SerializeField] private float _moveSpeed = 8f; public float moveSpeed { get => _moveSpeed; }
    [SerializeField] private float _airMoveSpeed = 8f; public float airMoveSpeed { get => _airMoveSpeed; }
    [SerializeField] private float _jumpForce = 15; public float jumpForce { get => _jumpForce; }
    [SerializeField] private float _wallJumpForce = 5f; public float wallJumpForce { get => _wallJumpForce; }
    [Range(0,1)]
    [SerializeField] public float _wallSlideSpeed = 0.5f; public float wallSlideSpeed { get => _wallSlideSpeed; }
    [SerializeField] public bool _canWallSlide; public bool canWallSlide { get => _canWallSlide; }
    public int facingDirection { get; private set; }
    [SerializeField] private bool _canMove = true; public bool canMove{ get => _canMove; }
    public int moveDirection { get; private set; }

    [Header(" Dash ")]
    [SerializeField] private float _dashSpeed; public float dashSpeed { get => _dashSpeed; private set => _dashSpeed = value; }
    [SerializeField] private float _dashDuration; public float dashDuration{ get => _dashDuration; private set => _dashDuration = value; }
    [SerializeField] private float _dashCooldown; public float dashCooldown{ get => _dashCooldown; private set => _dashCooldown = value; }
    [HideInInspector] private int _dashDirection = 1; public int dashDirection { get => _dashDirection; private set => _dashDirection = value; }
    [SerializeField] private bool _canDash = true; public bool canDash { get => _canDash; private set => _canDash = value; }

    [Header(" Ground Collision ")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private float groundCastDistance;


    [Header(" Wall Collision ")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCastDistance;

    [Header(" Attack Settings ")]
    [Tooltip("The amount in this field should be the same amount as attacks you have, this will make the player move a bit per attack ")]
    [SerializeField] public float[] attackMovement;

    // Misc
    public bool isBusy {  get; private set; }

    #region [--- Components ---]
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    private PlayerStateMachine stateMachine;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        facingDirection = 1;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new PlayerStateMachine();
        stateMachine.Initialize();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float vX, float vY)
    {
        rb.velocity = new Vector2(vX, vY);
    }


    private void OnMovePerformed()
    {
        if (canMove)
        {
            SetXDirection();
            SetFacingDirection();
            Flip();
        }

    }

    private void OnMoveCanceled()
    {
        moveDirection = 0;
        Debug.Log("Stopped Moving");
    }

    public void Flip()
    {
        transform.localScale = new Vector3(1 * facingDirection, 1, 1);
        dashDirection = facingDirection == 0 ? 1 : facingDirection;
    }

    private void SetXDirection()
    {
        moveDirection = (int)PlayerInputManager.Instance.moveVector.x;
    }

    private void SetFacingDirection()
    {
        if (moveDirection != 0)
            facingDirection = moveDirection;
    }

    public void SetFacingDirection(int direction)
    {
        if (direction != 0)
            facingDirection = direction;
    }


    #region Triggers / Events
    public void EnableMovement() => _canMove = true;
    public void DisableMovement() => _canMove = false;
    public void EnableDash() => _canDash = true;
    public void DisableDash() => _canDash = false;
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    #endregion

    #region Initalization
    
    private void OnEnable()
    {
        PlayerInputManager.movePerformed += OnMovePerformed;
        PlayerInputManager.moveCanceled += OnMoveCanceled;

    }

    private void OnDisable()
    {
        PlayerInputManager.movePerformed -= OnMovePerformed;
        PlayerInputManager.moveCanceled -= OnMoveCanceled;

    }

    #endregion

    #region Checks & Gizmos
    public bool isGrounded() => Physics2D.BoxCast(transform.position, groundCheckBoxSize, 0, -transform.up, groundCastDistance, whatIsGround); // GroundCheck
    public bool isWallDetected() => Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCastDistance, whatIsWall);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundCheckBoxSize); // GroundCheck
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCastDistance * facingDirection, transform.position.y)); // WallCheck
    }

    #endregion
}