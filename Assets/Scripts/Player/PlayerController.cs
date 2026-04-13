using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // PUBLIC VARIABLES
    public float defaultMoveSpeed = 2f;

    // ANIMATION BOOLEANS
    // public bool IsMoving;
    // public bool IsSprinting;
    // public bool IsIdle;

    public float moveSpeed = 2f;
    public float sprintSpeed = 4f;
    public float crouchSpeed = 1f;
    public float turnSpeed = 5f;

    private PlayerActions playerInput;
    public InputAction movement;
    private CharacterController characterController;
    private Animator animator;

    private void Awake()
    {
        playerInput = new PlayerActions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        movement = playerInput.PlayerMovement.Move;
        movement.Enable();

        movement.performed += ctx => PlayerMove();
        movement.canceled += ctx => PlayerStopMoving();

        playerInput.PlayerMovement.Sprint.performed += ctx => PlayerSprint();
        playerInput.PlayerMovement.Sprint.canceled += ctx => PlayerStopSprinting();
        playerInput.PlayerMovement.Sprint.Enable();

        playerInput.PlayerMovement.Crouch.performed += ctx => PlayerCrouch();
        playerInput.PlayerMovement.Crouch.canceled += ctx => PlayerStopCrouching();
        playerInput.PlayerMovement.Crouch.Enable();
    }

    void OnDisable()
    {
        movement.Disable();

        movement.performed -= ctx => PlayerMove();
        movement.canceled -= ctx => PlayerStopMoving();

        playerInput.PlayerMovement.Sprint.performed -= ctx => PlayerSprint();
        playerInput.PlayerMovement.Sprint.canceled -= ctx => PlayerStopSprinting();
        playerInput.PlayerMovement.Sprint.Disable();

        playerInput.PlayerMovement.Crouch.performed -= ctx => PlayerCrouch();
        playerInput.PlayerMovement.Crouch.canceled -= ctx => PlayerStopCrouching();
        playerInput.PlayerMovement.Crouch.Disable();
    }

    void Update()
    {
    }

    void PlayerMove()
    {
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsIdle", false);
        
        Debug.Log("Player is walking");
    }

    void PlayerStopMoving()
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsIdle", true);
    }

    void PlayerSprint()
    {
        animator.SetBool("IsSprinting", true);
    }

    void PlayerStopSprinting()
    {
        animator.SetBool("IsSprinting", false);
    }

    void PlayerCrouch()
    {
        animator.SetBool("IsCrouching", true);
    }

    void PlayerStopCrouching()
    {
        animator.SetBool("IsCrouching", false);
    }
}
