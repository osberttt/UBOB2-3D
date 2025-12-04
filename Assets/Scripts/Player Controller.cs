using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset inputActionAsset;

    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 move;

    private void OnEnable()
    {
        inputActionAsset.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActionAsset.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    public float speed = 5f;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
        move.Normalize();
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.y * speed);

        if (jumpAction.WasPressedThisFrame()) // Input.GetButtonDown("Jump")
        {
            Debug.Log("Jump");
        }
        
    }
}
