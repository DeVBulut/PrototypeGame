using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackNum2State : PlayerState
{
    float axis;
    private Vector2 _dashingDir;
    private float lastImageXpos;
    private bool attackNext;
    public PlayerAttackNum2State(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {

    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void FrameUpdate()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.GroundAttack_2) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void EnterState()
    {
        playerController.canMove = false;
        rb.velocity = Vector3.zero;
        animator.Play(Anim.GroundAttack_2);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        playerController.canMove = true;
    }
}
