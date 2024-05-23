using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentPlayerState { get; set; }

    public void Initialize(PlayerState playerState)
    {
        CurrentPlayerState = playerState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentPlayerState.ExitState(newState);
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }
}
