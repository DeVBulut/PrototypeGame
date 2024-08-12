using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSleepState : PlayerState
{
    public PlayerSleepState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void EnterState()
    {
        playerController.canMove = false;
        animator.Play(Anim.Sleep_Start);
        Debug.Log(this.ToString());
    }

    public override void FrameUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Sleep_Start) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            animator.Play(Anim.Sleeping);
        }
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.E))
        {
            animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Resting);
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
