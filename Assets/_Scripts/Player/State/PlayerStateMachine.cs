using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit(); // Ends current state
        currentState = newState; // Sets new state
        currentState.Enter(); // Enters / Starts the new state
    }

}