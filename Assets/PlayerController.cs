using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private bool alive = true;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float airSpeed = 7f;
    private float InputAxis;
    private bool smoothingSwitch;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI outputField0;
    public TextMeshProUGUI outputField1;
    public TextMeshProUGUI outputField2;
    private BoxCollider2D groundCheck;

    #region State Machine Variables 

    public PlayerStateMachine stateMachine { get; set; }
    public PlayerIdleState idleState { get; set; }
    public PlayerRunState runState { get; set; }

    #endregion

    public string playerState;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine);
        runState = new PlayerRunState(this, stateMachine);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheck = transform.GetChild(0).GetComponent<BoxCollider2D>();
        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        //DetermineState();
        stateMachine.CurrentPlayerState.FrameUpdate();
        Flip();
        ControlDeveloperPanel();
    }

    void FixedUpdate()
    {
        stateMachine.CurrentPlayerState.PhysicsUpdate();
        Move();
    }
    
    public void DetermineState()
    {
        if (!alive)
        {
            StartState(VariableList.STATE_DEAD);
        }
        //Ground State Selection
        else if (isGrounded() == true) 
        {
            //Determine if Idle
            if (Mathf.Abs(InputAxis) != 1 && Mathf.Abs(rb.velocity.x) < 0.2f)
            {
                StartState(VariableList.STATE_IDLE);
            }
            //Determine if either turning or accelerating start
            else if (InputAxis != 0 && Mathf.Abs(rb.velocity.x) < 1.5f)
            {
                if(playerState == VariableList.STATE_RUNNING)
                {
                    StartState(VariableList.STATE_RUN_TURN);
                }
                else if (playerState != VariableList.STATE_RUNNING && playerState != VariableList.STATE_RUN_TURN) 
                {
                    StartState(VariableList.STATE_RUN_START);
                } 
                
            }
            //Determine if Stopping
            else if (InputAxis == 0 && Mathf.Abs(rb.velocity.x) < 1.5f && Mathf.Abs(rb.velocity.x) > 0.2f)
            {
                StartState(VariableList.STATE_RUN_STOP);
            }
            //Determine if Running
            else if (Mathf.Abs(rb.velocity.x) > 0.8f)
            {
                if (InputAxis != 0)
                {
                    StartState(VariableList.STATE_RUNNING);
                }
            }
        }
        //Air State Selection
        else if (!isGrounded())
        {
            if (rb.velocity.y < 0.5 && rb.velocity.y > -0.5)
            {
                StartState(VariableList.STATE_AIR_PEAK);
            }
            else if (rb.velocity.y > 0)
            {
                StartState(VariableList.STATE_AIR_RISE);
            }
            else if (rb.velocity.y < 0)
            {
                StartState(VariableList.STATE_AIR_FALL);
            }
        }
    }

    public void StartState(string state)
    {
        //update State Physics Continously
        UpdateState(state);

        //if state is not the same as before -> update the playerState
        if (playerState != state) 
        {
            //IF CURRENT STATE IS RUN START AND NEW STATE IS RUNNING THEN WAIT FOR RUN START TO FINISH
            if(playerState == VariableList.STATE_RUN_START && state == VariableList.STATE_RUNNING)
            {
                //Wait for animation to finish
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99) 
                {
                    playerState = state;
                } 
                //if not finished return
                else 
                {
                    return;
                }
            }
            //IF CURRENT STATE ISN'T TURNING THEN PUSH NEXT STATE
            else if (playerState != VariableList.STATE_RUN_TURN) 
            {
                //Modify the current state by pushing desired state
                playerState = state;
            }
            //IF CURRENT STATE IS TURNING AND CHARACTER IS GROUNDED THEN IGNORE OTHER STATE REQUESTS
            else if(playerState == VariableList.STATE_RUN_TURN && isGrounded())
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99) 
                {
                    playerState = state;
                } 
                else 
                {
                    return;
                }
            }
        }
        //if state is the same as playerState -> don't do anything
        else {return; }

        Debug.Log(state + " ||  InputAxis is :" + InputAxis);
        
        switch (state)
        {
            case VariableList.STATE_DEAD: 
            {
                Debug.Log("Character Dead");
                runSpeed = 0;
                break;
            }

            case VariableList.STATE_IDLE:
            {
                animator.Play(state);
                break;
            }

            case VariableList.STATE_RUNNING:
            {
                animator.Play(state);
                break;
            }

            case VariableList.STATE_AIR_RISE:
            {
                animator.Play(state);       
                break;
            }

            case VariableList.STATE_AIR_PEAK:
            {
                animator.Play(state);
                break;
            }

            case VariableList.STATE_AIR_FALL:
            {
                animator.Play(state);
                break;
            }

            default:{
                animator.Play(state);
                break;
            }
        }

    }

    public void UpdateState(string state)
    {
        switch (state){
            case VariableList.STATE_DEAD: 
            {
                    break;
            }

            case VariableList.STATE_IDLE:
            {
                    //Play Idle
                    break;
            }

            case VariableList.STATE_AIR_RISE:
            {
                 //Play Rise
                break;
            }

            case VariableList.STATE_AIR_PEAK:
            {
                //Play Peak
                break;
            }

            case VariableList.STATE_AIR_FALL:
            {
                 //Play Fall
                break;
            }
        }
    }

    //Movement Code
    public void Move() 
    {

        //Input Axis
        InputAxis = Input.GetAxisRaw("Horizontal");
        
        //Velocities for Movement
        float variableSpeed = isGrounded() == true ? runSpeed : airSpeed ;
        float variableSmoothing = VariableList.movementSmoothing + (InputAxis != 0 && smoothingSwitch == false && isGrounded() ? VariableList.movementSmoothing : 0f);

        
        Vector2 targetVelocity = new Vector2(InputAxis * variableSpeed, rb.velocity.y);
        
        //zero variable
        Vector3 zeroVector = Vector2.zero;  
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, isGrounded() ? variableSmoothing : variableSmoothing + 0.1f);

    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && isGrounded()){ 

            //add force in positive upwards direction
            rb.AddForce(Vector2.up * VariableList.jumpPower);
        }
    }

    public void Flip()
    {
        if (playerState != VariableList.STATE_RUNNING) 
        {
            if (InputAxis < 0f)
            {
                if (spriteRenderer.flipX == false)
                {
                    //StartState(VariableList.STATE_RUN_TURN);
                    spriteRenderer.flipX = true;
                }
            }
            else if (InputAxis > 0f)
            {
                if (spriteRenderer.flipX == true)
                {
                    //StartState(VariableList.STATE_RUN_TURN);
                    spriteRenderer.flipX = false;
                }
            }
        }
    }

    //Boolean Functions
    public bool isGrounded()
    {
        return (Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundLayer).Length > 0) ? true : false;
    }


    //Developer Functions
    public void ControlDeveloperPanel()
    {
        //Dev Console Set Value
        outputField0.text = "Grounded : " + isGrounded().ToString();
        outputField1.text = "State    : " + playerState;
        outputField2.text = "Velocity : " + Mathf.Abs(rb.velocity.x);
        if(Input.GetKeyDown(KeyCode.F1))
        {
            outputField0.enabled = !outputField0.enabled;
        }
    }
}
