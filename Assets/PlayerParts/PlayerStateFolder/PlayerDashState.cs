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
        rb.AddForce(new Vector2(40f * axis, 0));
    }

    public override void FrameUpdate()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_2) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            //rb.velocityX = rb.velocityX -2f;
            playerStateMachine.ChangeState(playerController.dashExitState);
        }
    }

    public override void EnterState()
    {
        playerController.canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY; 
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
        animator.Play(Anim.Dash_2);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        //playerController.canMove = true;
        // rb.constraints = RigidbodyConstraints2D.None;
        // rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }
}
