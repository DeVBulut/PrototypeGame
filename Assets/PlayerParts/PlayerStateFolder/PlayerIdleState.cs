using UnityEngine;

public class PlayerIdleState : PlayerState
{

    public PlayerIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rb) : base(playerController, playerStateMachine, animator, rb)
    {

    }

    public override void PhysicsUpdate()
    {
        Debug.Log(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Idle));
    }

    public override void FrameUpdate()
    {
        float InputAxis = Input.GetAxisRaw("Horizontal");
        if(InputAxis != 0 && Mathf.Abs(rb.velocity.x) < 1.5f)
        {
            playerStateMachine.ChangeState(playerController.runStartState);
        }
    }

    public override void EnterState()
    {
        animator.Play(Anim.Idle);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {

    }
}
