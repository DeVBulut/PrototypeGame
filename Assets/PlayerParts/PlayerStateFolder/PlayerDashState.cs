using UnityEngine;

public class PlayerDashState : PlayerState
{
    float axis;
    public SpriteRenderer sp;
    public PlayerDashState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void PhysicsUpdate()
    {
        rb.velocity = new Vector2( axis * 4, 0);
    }

    public override void FrameUpdate()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && this.playerController.isGrounded())
        {
            rb.velocity = new Vector2( axis * 1, 0);
            playerStateMachine.ChangeState(playerController.runStopState);
        }
        else if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && !this.playerController.isGrounded())
        {
            rb.velocity = new Vector2( axis * 1, 0);
            playerStateMachine.ChangeState(playerController.peakState);
        }
    }

    public override void EnterState()
    {
        //playerController.canMove = false;
        if(sp.flipX == true)
        {
            axis = -1;
        }   
        else
        {
            axis = 1;
        }
        rb.velocity = Vector2.zero;
        animator.Play(Anim.Dash);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
    }
}
