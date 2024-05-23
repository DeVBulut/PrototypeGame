using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void EnterState()
    {
        animator.Play(Anim.Fall);
        Debug.Log(this.ToString());
    }

    public override void FrameUpdate()
    {
        if(playerController.isGrounded())
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void ExitState(PlayerState newState)
    {

    }
}
