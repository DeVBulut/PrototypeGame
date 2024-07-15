using UnityEngine;

public class PlayerDashExitState : PlayerState
{
    public SpriteRenderer sp;
    public PlayerDashExitState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void PhysicsUpdate()
    {
        //rb.velocity = _dashingDir * 6f;
    }

    public override void FrameUpdate()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_Exit) && Input.GetAxisRaw("Horizontal") != 0)
        {
            playerStateMachine.ChangeState(playerController.runState);
        }
        else if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_Exit) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            if(playerController.isGrounded())
            {
                playerStateMachine.ChangeState(playerController.idleState);
            }
            else
            {
                playerStateMachine.ChangeState(playerController.fallState);
            }
        }
        
    }

    public override void EnterState()
    {
        animator.Play(Anim.Dash_Exit);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {

    }
}
