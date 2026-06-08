using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    // PUBLIC VARIABLES
    [Header("Player Movement Configurations")]
    public float defaultMoveSpeed = 2f;
    public float moveSpeed = 2f;
    public float sprintSpeed = 4f;
    public float crouchSpeed = 1f;
    public float turnSpeed = 5f;

    [Header("Player Revival Configurations")]
    // [Tooltip("Player's most recently interacted with checkpoint. Used for player revival functions. Players respawn at their most recently interacted checkpoint.")]
    // public Transform m_Checkpoint;

    // PLAYER DEATH SETTINGS
    [SerializeField] private float deathSequenceTime = 6f;
    // Prevents the death trigger from re-triggering consistently.
    private float m_deathCooldownTimer = 0;
    public bool deathAnimHasPlayed = false;
    public bool playerHasDied = false;

    [Header("Gravity Configurations")]
    [SerializeField] private float gravityVelocity = -9.81f;
    private float verticalVelocity;

    private Vector3 moveDirection;
    // public PlayerInput playerInput;
    // public InputAction movement;
    private CharacterController characterController;
    private Animator animator;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void LoadData(GameData data)
    {
        // this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        // data.playerPosition = this.transform.position;
        // data.hasLastSavedPlayerPosition = true;
        // Debug.Log("SaveData in PlayerController called.");
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Update()
    {
        Debug.Log("PlayerHasDied bool: " + playerHasDied);
        Debug.Log("Death Anim Has Played: " + deathAnimHasPlayed);

        if (DialogueManager.GetInstance().dialogueIsPlaying || GameSceneManager.instance.SceneIsLoading)
        {
            return;
        }

        if (!GameManager.instance.PlayerIsAlive)
        {
            if (!deathAnimHasPlayed)
            {
                animator.SetTrigger("Death");
                deathAnimHasPlayed = true;
            }
            playerHasDied = true;
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsSprinting", false);
            animator.SetBool("IsCrouching", false);
            return;
        }

        moveDirection = InputManager.GetInstance().GetMoveDirection();
        float inputMagnitude = moveDirection.sqrMagnitude;
        bool isMoving = inputMagnitude > 0.01f;
        bool isCrouching = InputManager.GetInstance().IsCrouching;
        bool isSprinting = isMoving && !isCrouching && InputManager.GetInstance().IsSprinting; 

        if (!isMoving)
        {
            moveSpeed = 0f;
            isSprinting = false;
        }
        else if (isCrouching)
        {
            moveSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }

        // Vector3 localMoveDirection = moveDirection.magnitude > 0 ? transform.TransformDirection(moveDirection.normalized) : Vector3.zero;
        // Vector3 finalMovement = localMoveDirection * moveSpeed;
        Vector3 finalMovement = moveDirection * moveSpeed;

        if (characterController.isGrounded)
        {
            verticalVelocity = -2f; 
        }
        else
        {
            verticalVelocity += gravityVelocity * Time.deltaTime;
        }

        finalMovement.y = verticalVelocity;

        characterController.Move(finalMovement * Time.deltaTime);

        if (isMoving)
        {
            gameObject.transform.rotation = Quaternion.Lerp(
                gameObject.transform.rotation, 
                Quaternion.LookRotation(moveDirection), 
                Time.deltaTime * turnSpeed
            );
        }

        animator.SetBool("IsMoving", isMoving);

        if (!isMoving)
        {
            animator.SetBool("IsSprinting", false);
            animator.SetBool("IsCrouching", false);
            animator.SetBool("IsIdle", true);
        }
        else
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsSprinting", isSprinting);
            animator.SetBool("IsCrouching", isCrouching);
        }
    }

    // public void RevivePlayer()
    // {
    //     deathAnimHasPlayed = false;
    // }

    // IEnumerator RevivePlayerSequence()
    // {
    //     yield return new WaitForSeconds(deathSequenceTime);
    // }
}
