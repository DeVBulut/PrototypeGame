using System;
using NUnit.Framework.Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //private bool alive = true;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float airSpeed = 4f;
    private float InputAxis;
    public bool canMove;
    public LayerMask groundLayer;
    public LayerMask restZoneLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI outputField0;
    public TextMeshProUGUI outputField1;
    public TextMeshProUGUI outputField2;
    private BoxCollider2D groundCheck;

    #region State Machine Variables 

    public PlayerStateMachine stateMachine;
    public PlayerIdleState idleState;
    public PlayerRunState runState;
    public PlayerRunStartState runStartState;
    public PlayerRunStopState runStopState;
    public PlayerRiseState riseState;
    public PlayerPeakState peakState;
    public PlayerFallState fallState;
    public PlayerTurnState turnState;
    public PlayerLandState landState;
    public PlayerRollState rollState;
    public PlayerDashState dashState;
    public PlayerRestState restState;
    public PlayerSleepState sleepState;
    public PlayerDashExitState dashExitState;
    public PlayerAttackNum1State playerAttackNum1;
    public PlayerAttackNum2State playerAttackNum2;
    public Transform pos;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheck = transform.GetChild(0).GetComponent<BoxCollider2D>();

        idleState.Setup(this, stateMachine, animator, rb);
        runState.Setup(this, stateMachine, animator, rb);
        runStartState.Setup(this, stateMachine, animator, rb);
        runStopState.Setup(this, stateMachine, animator, rb);
        riseState.Setup(this, stateMachine, animator, rb);
        peakState.Setup(this, stateMachine, animator, rb);
        fallState.Setup(this, stateMachine, animator, rb);
        turnState.Setup(this, stateMachine, animator, rb);
        landState.Setup(this, stateMachine, animator, rb);
        rollState.Setup(this, stateMachine, animator, rb);
        dashState.Setup(this, stateMachine, animator, rb);
        dashExitState.Setup(this, stateMachine, animator, rb);
        restState.Setup(this, stateMachine, animator, rb);
        sleepState.Setup(this, stateMachine, animator, rb);
        playerAttackNum1.Setup(this, stateMachine, animator, rb);
        playerAttackNum2.Setup(this, stateMachine, animator, rb);
    }

    void Start()
    {
        canMove = true;
        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.CurrentPlayerState.FrameUpdate();
        Flip();
        ControlDeveloperPanel();
        HandleResting();
        HandleCombat();
    }

    private void HandleResting()
    {
        Collider2D restingDisplayZone = Physics2D.OverlapCircle(this.transform.position, 1f, restZoneLayer);
        if(restingDisplayZone != null)
        {
            restingDisplayZone.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E) && stateMachine.CurrentPlayerState != restState)
            {
                stateMachine.ChangeState(restState);
            }
        }
    }

    private void HandleCombat()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && stateMachine.CurrentPlayerState != restState && stateMachine.CurrentPlayerState != playerAttackNum1 && stateMachine.CurrentPlayerState != playerAttackNum2) 
        {
            stateMachine.ChangeState(playerAttackNum1);
        }
    }
    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(pos.position, 1f);
    }

    void FixedUpdate()
    {
        stateMachine.CurrentPlayerState.PhysicsUpdate();
        Move();
    }
    
    //Movement Code
    public void Move() 
    {
        if (canMove) {

            //Input Axis
            InputAxis = Input.GetAxisRaw("Horizontal");

            //Velocities for Movement
            float variableSpeed = isGrounded() == true ? runSpeed : airSpeed;
            float variableSmoothing = VariableList.movementSmoothing + (InputAxis != 0 ? VariableList.movementSmoothing : 0f);


            Vector2 targetVelocity = new Vector2(InputAxis * variableSpeed, rb.velocity.y);

            //zero variable
            Vector3 zeroVector = Vector2.zero;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, isGrounded() ? variableSmoothing : variableSmoothing + 0.1f);
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && canJump()){ 

            //add force in positive upwards direction
            rb.AddForce(Vector2.up * VariableList.jumpPower);
            stateMachine.ChangeState(riseState);
        }
    }

    public void Dodge(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && canMove)
        { 
            if(isGrounded())
            {
                stateMachine.ChangeState(dashState);
            }else
            {
                stateMachine.ChangeState(dashState);
            }
        }
    }


    public void Flip()
    {
        if (InputAxis < 0f)
        {
            if (spriteRenderer.flipX == false)
            {

                if (isRunning()) { stateMachine.ChangeState(turnState); }
                spriteRenderer.flipX = true;
            }
        }
        else if (InputAxis > 0f)
        {
            if (spriteRenderer.flipX == true)
            {
                if (isRunning()) { stateMachine.ChangeState(turnState); }
                spriteRenderer.flipX = false;
            }
        }
        
    }

    //Boolean Functions
    public bool isGrounded()
    {
        return (Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0) ? true : false;
    }

    public bool canJump()
    {
        return isGrounded() && canMove;
    }

    public bool isRunning()
    {
        if(stateMachine.CurrentPlayerState == runState && Mathf.Abs(rb.velocity.x) > 2) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Developer Functions
    public void ControlDeveloperPanel()
    {
        //Dev Console Set Value
        outputField0.text = "Grounded : " + isGrounded().ToString();
        outputField1.text = "State    : " + stateMachine.CurrentPlayerState.ToString();
        outputField2.text = "x : " + Mathf.Round(Mathf.Abs(rb.velocity.x) * 100f) / 100f + " y : " + Mathf.Round(rb.velocity.y * 100f) / 100f;
        if(Input.GetKeyDown(KeyCode.F1))
        {
            outputField0.enabled = !outputField0.enabled;
            outputField1.enabled = !outputField1.enabled;
            outputField2.enabled = !outputField2.enabled;
        }
    }
}
