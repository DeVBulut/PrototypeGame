using Unity.Cinemachine;
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
        float InputAxis = Input.GetAxisRaw("Horizontal");
        if(InputAxis == 0 && playerController.isGrounded() && Mathf.Abs(rb.velocity.x) < 1f )
        {
            playerStateMachine.ChangeState(playerController.landState);
        }
        else if( InputAxis != 0 && playerController.isGrounded() &&  Mathf.Abs(rb.velocity.x) > 1f)
        {
            playerStateMachine.ChangeState(playerController.runState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void ExitState(PlayerState newState)
    {

    }
}
