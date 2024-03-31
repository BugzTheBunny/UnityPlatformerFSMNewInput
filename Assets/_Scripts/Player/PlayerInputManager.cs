using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    public Vector2 moveVector { get; private set; }

    public PlayerActionAsset actions { get; private set; }


    [Header(" Actions ")]
    public static Action jumpPerformed;
    public static Action actionPerformed;
    public static Action dashPerformed;
    public static Action movePerformed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SetupInputsOnEnable();
    }

    private void OnDisable()
    {
        SetupInputsOnDisable();
    }

    void Start()
    {
        actions = new PlayerActionAsset();
    }

    private void SetupInputsOnEnable()
    {
        actions = new PlayerActionAsset();
        actions.Enable();
        actions.Player.Move.performed += OnMovePerformed;
        actions.Player.Move.canceled += OnMoveCanceled;
        actions.Player.Jump.performed += OnJumpPerformed;
        actions.Player.Action.performed += OnActionPerformed;
        actions.Player.Dash.performed += OnDashPerformed;

    }



    private void SetupInputsOnDisable()
    {
        actions.Player.Move.performed -= OnMovePerformed;
        actions.Player.Move.canceled -= OnMoveCanceled;
        actions.Player.Jump.performed -= OnJumpPerformed;
        actions.Player.Action.performed -= OnActionPerformed;
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
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        actionPerformed?.Invoke();
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
