using UnityEngine;

public class PlayerLandState : PlayerState
{
    public PlayerLandState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {

    }

    public override void EnterState()
    {
        animator.Play(Anim.Land);
        Debug.Log(this.ToString());
    }

    public override void FrameUpdate()
    {
        float InputAxis = Input.GetAxisRaw("Horizontal");
        if(InputAxis == 0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
        else if(InputAxis != 0 && Mathf.Abs(rb.velocity.x) < 6f)
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
