using UnityEngine;

public class PlayerDashExitState : PlayerState
{
    float reverseAxis;
    bool done;
    public SpriteRenderer sp;
    public PlayerDashExitState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void PhysicsUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99) { rb.AddForce(new Vector2(8f * reverseAxis, 0)); }
    }

    public override void FrameUpdate()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_3) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99 && this.playerController.isGrounded())
        {
            done = true;
            //rb.velocityX = 0f;
            playerStateMachine.ChangeState(playerController.idleState);
        }
        else if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_3) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99 && !this.playerController.isGrounded())
        {
            done = true;
            //rb.velocityX = 0f;
            playerStateMachine.ChangeState(playerController.peakState);
        }
    }

    public override void EnterState()
    {
        done = false;
        if(sp.flipX == true)
        {
            reverseAxis = 1;
        }   
        else
        {
            reverseAxis = -1;
        }
        animator.Play(Anim.Dash_3);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        playerController.canMove = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }
}
