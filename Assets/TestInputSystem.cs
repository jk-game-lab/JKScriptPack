using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TestInputSystem : MonoBehaviour
{

    public Vector2 MoveTracking;
    public Vector2 LookTracking;

    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction lookAction;
    private InputAction moveAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
//        fireAction = playerInput.actions["fire"];
        lookAction = playerInput.actions["look"];
        moveAction = playerInput.actions["move"];
    }

    void Start()
    {
        
    }

    void Update()
    {
        MoveTracking = moveAction.ReadValue<Vector2>();
        LookTracking = lookAction.ReadValue<Vector2>();
    }
}
