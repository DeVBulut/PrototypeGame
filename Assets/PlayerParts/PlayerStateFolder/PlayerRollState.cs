using UnityEngine;

public class PlayerRollState : PlayerState
{
    float axis;
    public SpriteRenderer sp;
    public PlayerRollState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void PhysicsUpdate()
    {
        rb.velocity = new Vector2( axis * 3, rb.velocity.y);
    }

    public override void FrameUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.90)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void EnterState()
    {
        if(sp.flipX == true)
        {
            axis = -1;
        }   
        else
        {
            axis = 1;
        }
        playerController.canMove = false;
        rb.velocity = Vector2.zero;
        animator.Play(Anim.Roll);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        rb.velocity = Vector2.zero;
        playerController.canMove = true;
    }
}
