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
    //private bool alive = true;
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
    public PlayerRunStartState runStartState { get; set; }
    public PlayerRunStopState runStopState { get; set; }
    public PlayerRiseState riseState { get; set; }
    public PlayerPeakState peakState { get; set; }
    public PlayerFallState fallState { get; set; }


    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheck = transform.GetChild(0).GetComponent<BoxCollider2D>();

        stateMachine =  new PlayerStateMachine();
        idleState =     new PlayerIdleState(this, stateMachine, animator, rb);
        runState =      new PlayerRunState(this, stateMachine, animator, rb);
        runStartState = new PlayerRunStartState(this, stateMachine, animator, rb);
        runStopState =  new  PlayerRunStopState(this, stateMachine, animator, rb);
        riseState =     new  PlayerRiseState(this, stateMachine, animator, rb);
        peakState =     new  PlayerPeakState(this, stateMachine, animator, rb);
        fallState =     new  PlayerFallState(this, stateMachine, animator, rb);
    }

    void Start()
    {
        stateMachine.Initialize(idleState);
    }

    void Update()
    {
        stateMachine.CurrentPlayerState.FrameUpdate();
        Flip();
        ControlDeveloperPanel();
    }

    void FixedUpdate()
    {
        stateMachine.CurrentPlayerState.PhysicsUpdate();
        Move();
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
            stateMachine.ChangeState(riseState);
        }
    }

    public void Flip()
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
        outputField1.text = "State    : " + stateMachine.CurrentPlayerState.ToString();
        outputField2.text = "x : " + Mathf.Round(Mathf.Abs(rb.velocity.x) * 100f) / 100f + " y : " + Mathf.Round(rb.velocity.y * 100f) / 100f;
        if(Input.GetKeyDown(KeyCode.F1))
        {
            outputField0.enabled = !outputField0.enabled;
        }
    }
}
