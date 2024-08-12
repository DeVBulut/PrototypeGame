using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackNum1State : PlayerState
{
    float axis;
    private Vector2 _dashingDir;
    private float lastImageXpos;
    private bool attackNext;
    public PlayerAttackNum1State(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {

    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void FrameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attackNext = true;
        }

        if(attackNext == true && this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.GroundAttack_1) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7)
        {
            playerStateMachine.ChangeState(playerController.playerAttackNum2);
        }

        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.GroundAttack_1) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            playerStateMachine.ChangeState(playerController.idleState);
        }
    }

    public override void EnterState()
    {
        attackNext = false;
        playerController.canMove = false;
        rb.velocity = Vector3.zero;
        animator.Play(Anim.GroundAttack_1);
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        playerController.canMove = true;
    }
}
