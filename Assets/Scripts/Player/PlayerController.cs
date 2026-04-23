using Unity.VisualScripting;
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

    private Vector3 moveDirection;
    // private Vector3 lastMoveDirection;

    public PlayerInput playerInput;
    public InputAction movement;
    private CharacterController characterController;
    private Animator animator;

    // public bool playerInteracted = false;

    private void Awake()
    {
        // playerInput = new PlayerActions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // playerInteracted = false;
    }

    void OnEnable()
    {
        // movement = playerInput.PlayerControls.Move;
        // movement.Enable();

        // movement.performed += ctx => PlayerMove();
        // movement.canceled += ctx => PlayerStopMoving();

        // playerInput.PlayerControls.Sprint.performed += ctx => PlayerSprint();
        // playerInput.PlayerControls.Sprint.canceled += ctx => PlayerStopSprinting();
        // playerInput.PlayerControls.Sprint.Enable();

        // playerInput.PlayerControls.Crouch.performed += ctx => PlayerCrouch();
        // playerInput.PlayerControls.Crouch.canceled += ctx => PlayerStopCrouching();
        // playerInput.PlayerControls.Crouch.Enable();

        // playerInput.PlayerControls.Interact.performed += ctx => PlayerInteract();
        // playerInput.PlayerControls.Interact.canceled += ctx => PlayerStopInteract();
        // playerInput.PlayerControls.Interact.Enable();
    }

    void OnDisable()
    {
        // movement.Disable();

        // movement.performed -= ctx => PlayerMove();
        // movement.canceled -= ctx => PlayerStopMoving();

        // playerInput.PlayerControls.Sprint.performed -= ctx => PlayerSprint();
        // playerInput.PlayerControls.Sprint.canceled -= ctx => PlayerStopSprinting();
        // playerInput.PlayerControls.Sprint.Disable();

        // playerInput.PlayerControls.Crouch.performed -= ctx => PlayerCrouch();
        // playerInput.PlayerControls.Crouch.canceled -= ctx => PlayerStopCrouching();
        // playerInput.PlayerControls.Crouch.Disable();

        // playerInput.PlayerControls.Interact.performed -= ctx => PlayerInteract();
        // playerInput.PlayerControls.Interact.canceled -= ctx => PlayerStopInteract();
        // playerInput.PlayerControls.Interact.Disable();
    }

    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        
        if (!InputManager.GetInstance().IsSprinting && !InputManager.GetInstance().IsCrouching)
        {
            moveSpeed = defaultMoveSpeed;
        }
        else if (InputManager.GetInstance().IsSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else if (InputManager.GetInstance().IsCrouching)
        {
            moveSpeed = crouchSpeed;
        }

        moveDirection = InputManager.GetInstance().GetMoveDirection();
        characterController.Move(InputManager.GetInstance().GetMoveDirection() * Time.deltaTime * moveSpeed);
        // gameObject.transform.LookAt(animator.gameObject.transform.position + moveDirection);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * turnSpeed);
        }
    }

    // void PlayerInteract()
    // {
    //     playerInteracted = true;
    //     // float interactRange = 2f;
    //     // Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
    //     // foreach (Collider collider in colliderArray)
    //     // {
    //     //     if (collider.TryGetComponent(out NPCInteractable npcInteractable)) {
    //     //         npcInteractable.Interact();
    //     //     }
    //     //     // Debug.Log(collider);
    //     // }
    // }

    // void PlayerStopInteract()
    // {
    //     playerInteracted = false;
    // }

    // void PlayerMove()
    // {
    //     animator.SetBool("IsMoving", true);
    //     animator.SetBool("IsIdle", false);

    //     Debug.Log("Player is walking");
    // }

    // void PlayerStopMoving()
    // {
    //     animator.SetBool("IsMoving", false);
    //     animator.SetBool("IsIdle", true);
    // }

    // void PlayerSprint()
    // {
    // }

    // void PlayerStopSprinting()
    // {
    //     animator.SetBool("IsSprinting", false);
    // }

    // void PlayerCrouch()
    // {
    //     animator.SetBool("IsCrouching", true);
    // }

    // // void PlayerStopCrouching()
    // // {
    // //     animator.SetBool("IsCrouching", false);
    // // }

    // void HandleMovement()
    // {
    //     Vector3 Direction = InputManager.GetInstance().GetMoveDirection();
    //     // characterController = animator.gameObject.GetComponent<CharacterController>();
    //     // PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();

    //     // Vector3 Direction = new(playerController.movement.ReadValue<Vector2>().x, 0, playerController.movement.ReadValue<Vector2>().y);

    //     characterController.Move(Direction * Time.deltaTime * moveSpeed);
    //     // animator.gameObject.transform.rotation = Quaternion.Lerp(animator.gameObject.transform.rotation, Quaternion.LookRotation(Direction), Time.deltaTime * playerController.turnSpeed);
    // }

    // public void MovePressed(InputAction.CallbackContext context)
    // {
    //     moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);

    //     if (context.performed)
    //     {
    //         animator.SetBool("IsMoving", true);
    //         animator.SetBool("IsIdle", false);
    //     }
    //     else if (context.canceled)
    //     {
    //         animator.SetBool("IsMoving", false);
    //         animator.SetBool("IsIdle", true);
    //     }
    // }

    // public void HandleSprint(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         moveSpeed = sprintSpeed;
    //         animator.SetBool("IsSprinting", true);

    //     }
    //     else if (context.canceled)
    //     {
    //         moveSpeed = defaultMoveSpeed;
    //         animator.SetBool("IsSprinting", false);

    //     }
    // }

    // public void HandleCrouch(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         moveSpeed = crouchSpeed;
    //         animator.SetBool("IsCrouching", true);
    //     }
    //     else if (context.canceled)
    //     {
    //         moveSpeed = defaultMoveSpeed;
    //         animator.SetBool("IsCrouching", false);
    //     }
    // }

}
