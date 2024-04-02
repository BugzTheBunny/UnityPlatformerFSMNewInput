using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public Vector2 moveVector { get; private set; }


    [Header(" Components ")]
    private PlayerActionAsset actions;


    [Header(" Actions ")]
    public static Action jumpPerformed;
    public static Action attackPerformed;
    public static Action dashPerformed;
    public static Action movePerformed;
    public static Action moveCanceled;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsibscribe();
    }

    void Start()
    {
        actions = new PlayerActionAsset();
    }

    private void Subscribe()
    {
        actions = new PlayerActionAsset();
        actions.Enable();
        actions.Player.Move.performed += OnMovePerformed;
        actions.Player.Move.canceled += OnMoveCanceled;
        actions.Player.Jump.performed += OnJumpPerformed;
        actions.Player.Attack.performed += OnAttackPerformed;
        actions.Player.Dash.performed += OnDashPerformed;

    }



    private void Unsibscribe()
    {
        actions.Player.Move.performed -= OnMovePerformed;
        actions.Player.Move.canceled -= OnMoveCanceled;
        actions.Player.Jump.performed -= OnJumpPerformed;
        actions.Player.Attack.performed -= OnAttackPerformed;
        actions.Player.Dash.performed -= OnDashPerformed;
        actions.Disable();
    }


    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        movePerformed?.Invoke();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
        moveCanceled?.Invoke();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        attackPerformed?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpPerformed?.Invoke();
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        dashPerformed?.Invoke();
    }
}
