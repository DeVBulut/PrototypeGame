using UnityEngine;

public class PlayerTurnState : PlayerState
{
    public PlayerTurnState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rb) : base(playerController, playerStateMachine, animator, rb)
    {
    }

    public override void PhysicsUpdate()
    {

    }

    public override void FrameUpdate()
    {
        float InputAxis = Input.GetAxisRaw("Horizontal");
        if(InputAxis != 0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            playerStateMachine.ChangeState(playerController.runState);
        }
        else if(InputAxis == 0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            playerStateMachine.ChangeState(playerController.runStopState);
        }
    }

    public override void EnterState()
    {
        animator.Play(Anim.Turn);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {

    }
}
