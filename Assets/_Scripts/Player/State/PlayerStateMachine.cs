using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    /// <summary>
    /// Changes the state
    /// </summary>
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit(); // Ends current state
        currentState = newState; // Sets new state
        currentState.Enter(); // Enters / Starts the new state
    }
}