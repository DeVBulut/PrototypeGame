using UnityEngine;

public class PlayerRestState : PlayerState
{
    public PlayerRestState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void EnterState()
    {
        rb.velocity = Vector2.zero;
        playerController.canMove = false;
        animator.Play(Anim.Rest_Start);
        Debug.Log(this.ToString());
    }

    public override void FrameUpdate()
    {

        if(animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Rest_Start) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            animator.Play(Anim.Resting);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 10 && animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Resting))
        {
            playerStateMachine.ChangeState(playerController.sleepState);
        }

        if(Input.anyKeyDown && animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Resting) && !Input.GetKeyDown(KeyCode.E))
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void ExitState(PlayerState newState)
    {
        playerController.canMove = true;
    }
}
