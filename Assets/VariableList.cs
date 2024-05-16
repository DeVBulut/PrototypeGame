using UnityEngine;
public static class VariableList
{
    // Player states
    public const string STATE_DEAD = "Dead";
    public const string STATE_IDLE = "Idle"; 
    public const string  STATE_RUN_START = "Run_Start";
    public const string  STATE_RUNNING = "Running";
    public const string  STATE_RUN_STOP = "Run_Stop";
    public const string  STATE_RUN_TURN = "Run_Turn";
    public const string STATE_AIR_RISE = "Rise";
    public const string STATE_AIR_PEAK = "Peak";
    public const string STATE_AIR_FALL = "Fall";
    
    //Player Values
    public static float DefaultHealth = 10;

    //public const float runSpeed = 10f;
    //public const float airSpeed = 8f;
    public const float movementSmoothing = 0.06f;
    public const float jumpPower = 250f;
    public const float maxCoyote = .2f;

}
