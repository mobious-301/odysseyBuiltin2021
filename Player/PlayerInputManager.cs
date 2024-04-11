using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using InputActionAsset;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    public InputActionAsset actions;
    protected InputAction m_movement;
    // protected InputAction m_movement;
    protected InputAction m_run;
    protected InputAction m_jump;
    protected InputAction m_dive;
    protected InputAction m_spin;
    protected InputAction m_pickAndDrop;
    protected InputAction m_crouch;
    protected InputAction m_airDive;
    protected InputAction m_stomp;
    protected InputAction m_releaseLedge;
    protected InputAction m_pause;
    protected InputAction m_look;
    protected InputAction m_glide;
    protected InputAction m_dash;
    protected InputAction m_grindBrake;

    protected float m_movementDirectionUnlockTime;
    protected virtual void CacheActions()
    {
        m_movement = actions["Movement"];
        m_movement = actions["Movement"];
        m_run = actions["Run"];
        m_jump = actions["Jump"];
        m_dive = actions["Dive"];
        m_spin = actions["Spin"];
        m_pickAndDrop = actions["PickAndDrop"];
        m_crouch = actions["Crouch"];
        m_airDive = actions["AirDive"];
        m_stomp = actions["Stomp"];
        m_releaseLedge = actions["ReleaseLedge"];
        m_pause = actions["Pause"];
        m_look = actions["Look"];
        m_glide = actions["Glide"];
        m_dash = actions["Dash"];
        m_grindBrake = actions["Grind Brake"];
    }

    protected virtual void Awake() => CacheActions();
    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        actions.Enable();
    }
    protected virtual void OnEnable() => actions?.Enable();
    protected virtual void OnDisable() => actions?.Disable();

    // Update is called once per frame

    //获取移动方向
    public virtual Vector3 GetMovementDirection()
    {
        if (Time.time < m_movementDirectionUnlockTime) return Vector3.zero;

        var value = m_movement.ReadValue<Vector2>();
        return GetAxisWithCrossDeadZone(value);
    }
    //获取移动方向
    public virtual Vector3 GetAxisWithCrossDeadZone(Vector2 axis)
    {
        var deadzone = InputSystem.settings.defaultDeadzoneMin;
        axis.x = Mathf.Abs(axis.x) > deadzone ? RemapToDeadzone(axis.x, deadzone) : 0;
        axis.y = Mathf.Abs(axis.y) > deadzone ? RemapToDeadzone(axis.y, deadzone) : 0;
        return new Vector3(axis.x, 0, axis.y);
    }
    protected float RemapToDeadzone(float value, float deadzone) => (value - deadzone) / (1 - deadzone);
    protected Camera m_camera;
    internal Vector3 GetMovementCameraDirection()
    {
        var direction = GetMovementDirection();

        if (direction.sqrMagnitude > 0)
        {
            var rotation = Quaternion.AngleAxis(m_camera.transform.eulerAngles.y, Vector3.up);
            direction = rotation * direction;
            direction = direction.normalized;
        }

        return direction;
    }
    public virtual bool GetRun() => m_run.IsPressed();
    public virtual bool GetRunUp() => m_run.WasReleasedThisFrame();
    protected float? m_lastJumpTime;
    protected const float k_jumpBuffer = 0.15f;
    public virtual bool GetJumpDown()
    {
        if (m_lastJumpTime != null &&
            Time.time - m_lastJumpTime < k_jumpBuffer)
        {
            m_lastJumpTime = null;
            return true;
        }

        return false;
    }

    public virtual void LockMovementDirection(float duration = 0.25f)
    {
        m_movementDirectionUnlockTime = Time.time + duration;
    }
    void Update()
    {
        if (m_jump.WasPressedThisFrame())
        {
            m_lastJumpTime = Time.time;
        }

    }

    public virtual bool GetJumpUp() => m_jump.WasReleasedThisFrame();//新输入系统 up  被 WasReleasedThisFrame替代

}
