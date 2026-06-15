using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Toggle to allow music and sound effects.")]
    [SerializeField] bool PlayMusic;
    [Tooltip("Source for background music. Can be null if none.")]
    [SerializeField] private AudioSource backgroundMusicSource = null;
    [Tooltip("Source for ambience music. Can be null if none.")]
    [SerializeField] private AudioSource ambienceMusicSource = null;
    [Tooltip("Source for player footstep sounds. Can be null if none.")]
    [SerializeField] private AudioSource playerFootstepSource = null;
    [Tooltip("Source for player running footstep sounds. Can be null if none.")]
    [SerializeField] private AudioSource playerRunFootstepSource = null;
    [Tooltip("Source for player heartbeat sounds. Can be null if none.")]
    [SerializeField] private AudioSource playerHeartbeatSource = null;

    void Update()
    {
        if (!PlayMusic)
            return;

        if (backgroundMusicSource != null && backgroundMusicSource.clip != null && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
        }

        if (ambienceMusicSource != null && ambienceMusicSource.clip != null && !ambienceMusicSource.isPlaying)
        {
            ambienceMusicSource.Play();
        }

        InputManager inputManager = InputManager.GetInstance();
        bool isMoving = inputManager != null && inputManager.IsMoving;
        bool isSprinting = inputManager != null && inputManager.IsSprinting;

        if (playerFootstepSource != null && playerFootstepSource.clip != null)
        {
            if (!playerFootstepSource.isPlaying && isMoving && !isSprinting)
            {
                Debug.Log("Playing footstep sound");
                playerFootstepSource.Play();
            }
            else if (playerFootstepSource.isPlaying && (isSprinting || !isMoving))
            {
                Debug.Log("Stopping footstep sound");
                playerFootstepSource.Stop();
            }
        }

        if (playerRunFootstepSource != null && playerRunFootstepSource.clip != null)
        {
            if (!playerRunFootstepSource.isPlaying && isSprinting)
            {
                Debug.Log("Playing running footstep sound");
                playerRunFootstepSource.Play();
            }
            else if (playerRunFootstepSource.isPlaying && !isSprinting)
            {
                Debug.Log("Stopping running footstep sound");
                playerRunFootstepSource.Stop();
            }
        }

        bool isBeingChased = GameManager.instance != null && GameManager.instance.PlayerIsBeingChased;

        if (playerHeartbeatSource != null && playerHeartbeatSource.clip != null)
        {
            if (!playerHeartbeatSource.isPlaying && isBeingChased)
            {
                Debug.Log("Playing heartbeat sound");
                playerHeartbeatSource.Play();
            }
            else if (playerHeartbeatSource.isPlaying && !isBeingChased)
            {
                Debug.Log("Stopping heartbeat sound");
                playerHeartbeatSource.Stop();
            }
        }
    }
}
