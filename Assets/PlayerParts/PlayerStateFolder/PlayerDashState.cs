using System;
using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    float axis;
    private Vector2 _dashingDir;
    private float lastImageXpos;
    public SpriteRenderer sp;
    public PlayerDashState(PlayerController playerController, PlayerStateMachine playerStateMachine, Animator animator, Rigidbody2D rigidbody) : base(playerController, playerStateMachine, animator, rigidbody)
    {
    }

    public override void PhysicsUpdate()
    {
        
        rb.velocity = _dashingDir * 5f;
    }

    public override void FrameUpdate()
    {
        if (Mathf.Abs(playerController.transform.GetComponent<Transform>().position.x - lastImageXpos) > 0.3f)
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            lastImageXpos = playerController.transform.GetComponent<Transform>().position.x;
        }

        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName(Anim.Dash_Start) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99)
        {
            animator.Play(Anim.Dashing);
        }
    }

    public override void EnterState()
    {
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        playerController.canMove = false;
        CheckAxis();


        _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(_dashingDir == Vector2.zero)
        {
            _dashingDir = new Vector2(axis, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        else if( _dashingDir == new Vector2(axis, 0))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }


        animator.Play(Anim.Dash_Start);
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = playerController.transform.GetComponent<Transform>().position.x;

        StartCoroutine(StopDashing());
        Debug.Log(this.ToString());
    }

    public override void ExitState(PlayerState newState)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(0.3f);
        playerController.canMove = true;
        rb.velocity = Vector2.zero;
        
        playerStateMachine.ChangeState(playerController.dashExitState);
    }

    private void CheckAxis()
    {
        if(sp.flipX == true)
        {
            axis = -1;
        }   
        else
        {
            axis = 1;
        }
    }
}
