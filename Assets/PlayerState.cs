using UnityEngine;

public class PlayerState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine)
    {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState()
    {

    }

    public virtual void FrameUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void ExitState()
    {

    }
}