using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector3 moveDirection = Vector2.zero;
    public bool IsMoving { get; private set; }
    public bool IsIdle { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsSprinting { get; private set; }

    private bool interactPressed = false;
    private bool submitPressed = false;

    private static InputManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput playerInput;

    private bool wasInteractPressed = false;
    private bool wasSubmitPressed = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one of this instance exists.");
        }
        else
        {
            instance = this;
            // animator = player.GetComponent<Animator>();
        }
    }

    void Update()
    {
        Debug.Log("Player is sprinting: " + IsSprinting);
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);

        if (context.performed)
        {
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsIdle", false);
            IsMoving = true;
            IsIdle = false;
        }
        else if (context.canceled)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", true);
            IsMoving = false;
            IsIdle = true;
        }
    }

    public void HandleSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // moveSpeed = sprintSpeed;
            IsSprinting = true;
            animator.SetBool("IsSprinting", true);

        }
        else if (context.canceled)
        {
            // moveSpeed = defaultMoveSpeed;
            IsSprinting = false;
            animator.SetBool("IsSprinting", false);
        }
    }

    public void HandleCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsCrouching = true;
            // moveSpeed = crouchSpeed;
            animator.SetBool("IsCrouching", true);
        }
        else if (context.canceled)
        {
            IsCrouching = false;
            // moveSpeed = defaultMoveSpeed;
            animator.SetBool("IsCrouching", false);
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Debug.Log("InteractPressed callback triggered!");
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public string GetCurrentlyActiveMap()
    {
        return playerInput.currentActionMap.name;
    }

    public void SwitchToUIMap()
    {
        // if (playerInput.currentActionMap != playerInput)
        playerInput.SwitchCurrentActionMap("UI_Input");
    }

    public void SwitchToPlayerMap()
    {
        playerInput.SwitchCurrentActionMap("PlayerControls");
    }

    // public void PlayerStopMoving(InputAction.CallbackContext context)
    // {
    //     animator.SetBool("IsMoving", false);
    //     animator.SetBool("IsIdle", true);
    // }
}
