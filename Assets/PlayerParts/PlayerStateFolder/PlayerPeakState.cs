using NUnit.Framework;
using UnityEngine;

public class PlayerPeakState : PlayerState
{
    public PlayerPeakState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void EnterState()
    {
        animator.Play(Anim.Peak);
        Debug.Log(this.ToString());
    }

    public override void FrameUpdate()
    {
        if(rb.velocity.y < -VariableList.peakThreshold && !playerController.isGrounded())
        {
            playerStateMachine.ChangeState(playerController.fallState);
        }
        else if(playerController.isGrounded())
        {
            playerStateMachine.ChangeState(playerController.landState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void ExitState(PlayerState newState)
    {

    }
}
