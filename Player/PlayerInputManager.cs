using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using InputActionAsset;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    public InputActionAsset actions;
    protected InputAction m_movement;
    protected virtual void CacheActions()
    {
        m_movement = actions["Movement"];
    }

    protected virtual void Awake() => CacheActions();
    // Start is called before the first frame update
    void Start()
    {
        actions.Enable();
    }
    protected virtual void OnEnable() => actions?.Enable();
	protected virtual void OnDisable() => actions?.Disable();

    // Update is called once per frame
    void Update()
    {
        
    }
}
