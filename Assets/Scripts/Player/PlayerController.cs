using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    // PUBLIC VARIABLES
    public float defaultMoveSpeed = 2f;

    public float moveSpeed = 2f;
    public float sprintSpeed = 4f;
    public float crouchSpeed = 1f;
    public float turnSpeed = 5f;

    private Vector3 moveDirection;

    public PlayerInput playerInput;
    public InputAction movement;
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

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * turnSpeed);
        }
    }
}
