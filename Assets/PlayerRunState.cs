using UnityEngine;

public class PlayerRunState : PlayerState
{

    public PlayerRunState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rb) : base(playerController, playerStateMachine, animator, rb)
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void FrameUpdate()
    {
        float InputAxis = Input.GetAxisRaw("Horizontal");
        if(InputAxis == 0)
        {
            playerStateMachine.ChangeState(playerController.runStopState);
        }
    }

    public override void EnterState()
    {
        animator.Play(Anim.Run);
        Debug.Log(this.ToString());
    }

    public override void ExitState()
    {

    }
}
