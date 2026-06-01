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
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
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
        }

        // if (playerHasDied)
        // {
        //     Debug.Log("Player has died. Initiating death and revival sequence.");
        //     m_deathCooldownTimer += Time.deltaTime;
        //     if (m_deathCooldownTimer >= deathSequenceTime)
        //     {
        //         GameManager.instance.RevivePlayer();
        //         animator.SetBool("IsIdle", true);
        //         deathAnimHasPlayed = false;
        //         playerHasDied = false;
        //         m_deathCooldownTimer = 0;
        //     }
        //     return;
        // }

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

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * turnSpeed);
        }

        animator.SetBool("IsMoving", InputManager.GetInstance().IsMoving);
        animator.SetBool("IsIdle", InputManager.GetInstance().IsIdle);
        animator.SetBool("IsSprinting", InputManager.GetInstance().IsSprinting);
        animator.SetBool("IsCrouching", InputManager.GetInstance().IsCrouching);
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
