using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Input System")]
    public InputActionAsset inputAsset;
    
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 1f;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Animator")]
    public Animator animator;
    
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 move;

    private Vector2 moveAmount;
    private Vector2 lookAmount;
    
    private void OnEnable()
    {
        inputAsset.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputAsset.FindActionMap("Player").Disable();
    }
    
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        HandleMovement();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // small downward force to keep grounded

        // Movement input
        moveAmount = moveAction.ReadValue<Vector2>();
        move = new Vector3(moveAmount.x, 0f, moveAmount.y).normalized;

        // Handle rotation and movement
        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float speed = sprintAction.IsPressed() ? runSpeed : walkSpeed;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleAnimation()
    {
        animator.SetBool("Walking", move.magnitude >= 0.1f);

        animator.SetBool("Sprinting", sprintAction.IsPressed());
    }
}