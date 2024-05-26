using System.Collections;
using System.Dynamic;
using UnityEngine;

public class PlayerRunStopState : PlayerState
{

    public PlayerRunStopState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rb) : base(playerController, playerStateMachine, animator, rb)
    {
    }

    public override void PhysicsUpdate()
    {

    }

    public override void FrameUpdate()
    {
        float InputAxis = Input.GetAxisRaw("Horizontal");

        if(InputAxis == 0 && Mathf.Abs(rb.velocity.x) < 1f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }

    }

    public override void EnterState()
    {
        animator.Play(Anim.Run_Stop); 
        Debug.Log(this.ToString());    
    }

    public override void ExitState(PlayerState newState)
    {

    }
}
