# 🧍‍♂️ Character Controller with State Machine – Unity Prototype

This project is a modular, extensible 2D character controller system designed in Unity, built using a formal **finite state machine architecture**. The system encapsulates animation handling, movement, combat states, and transitions through well-structured and isolated state classes. The controller emphasizes reusability, debuggability, and performance-conscious design.

> ⚠️ Uses non-commercial placeholder assets. Do not reuse visuals in commercial contexts.

---

## 🎯 Core Concepts

The controller is driven by a **central state machine**. Each gameplay behavior (e.g., jumping, landing, rolling) is abstracted into its own state class. This adheres to the **Open/Closed Principle** — the controller can be extended with new states without modifying core logic.

---

## 🧠 Architecture Summary

### `PlayerStateMachine.cs`
A lightweight FSM responsible for storing the active state and routing lifecycle events (`EnterState`, `FrameUpdate`, `PhysicsUpdate`, `ExitState`). States are derived from an abstract base `PlayerState` that implements shared references and methods.

### `PlayerController.cs`
A monolithic orchestrator acting as the interface between Unity's engine loop and the FSM. It manages input, physics, flip logic, and internal transitions between grounded and air states. All transitions are mediated through the `stateMachine`.

### State Definitions

Each state (e.g., `PlayerIdleState`, `PlayerFallState`, `PlayerAttackNum1State`) inherits from the common `PlayerState` base. Lifecycle methods are overridden per state to define behavior on enter, update, and exit. The architecture enforces **single-responsibility** by isolating behavior logic.

Example: `PlayerPeakState.cs`

```csharp
public override void FrameUpdate()
{
    if (rb.velocity.y < -VariableList.peakThreshold && !playerController.isGrounded())
    {
        playerStateMachine.ChangeState(playerController.fallState);
    }
    else if (playerController.isGrounded())
    {
        playerStateMachine.ChangeState(playerController.landState);
    }
}
```

---

## 🔧 Implemented States

This system currently supports **20+ states**:

- Movement: `Idle`, `RunStart`, `Run`, `RunStop`, `Turn`
- Air: `Rise`, `Peak`, `Fall`, `Land`
- Combat: `AttackNum1`, `AttackNum2`, `Roll`, `Dash`, `DashExit`
- Utility: `Rest`, `Sleep`

Each state independently manages its animation transitions, physics logic, and entry/exit conditions.

---

## 📦 System Behavior

### Input Handling

Input is indirectly interpreted through the controller's logic (e.g., `Jump`, `Dodge`, or `Mouse0` for combat), which defers state changes to the FSM. This **decouples input from behavior**, allowing rebinds or alternate input drivers without modifying logic.

### Developer Output Console

A live debug console shows:
- Current grounded status
- Current FSM state
- Velocity vectors (x, y)
Toggle: `F1`

### Movement

```csharp
Vector2 targetVelocity = new Vector2(InputAxis * variableSpeed, rb.velocity.y);
rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, smoothing);
```

Movement is implemented using `SmoothDamp` for responsive control with tunable smoothing and air-ground distinction.

---

## 🛠 Technologies Used

- Unity Engine (2D URP)
- C#
- Unity Animator and Physics2D
- Custom FSM pattern
- Debug instrumentation (TMP output, Gizmos)

---

## 🚧 Status

This system is in an active but paused state. The foundational state machine and controller logic is complete and functional. Future improvements could include:
- Modular behavior tree layering
- ScriptableObject-driven config per state

---

## 📜 License

MIT — Code is free to modify and extend.  
🚫 Placeholder visuals must not be redistributed or used commercially.
