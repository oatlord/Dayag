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
        if (PlayMusic)
        {
            if (backgroundMusicSource.clip != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }
            if (ambienceMusicSource.clip != null && !ambienceMusicSource.isPlaying)
            {
                ambienceMusicSource.Play();
            }

            if (playerFootstepSource.clip != null && !playerFootstepSource.isPlaying
            && InputManager.GetInstance().IsMoving && !InputManager.GetInstance().IsSprinting)
            {
                Debug.Log("Playing footstep sound");
                playerFootstepSource.Play();
            }
            else if (playerFootstepSource.isPlaying && (InputManager.GetInstance().IsSprinting || !InputManager.GetInstance().IsMoving))
            {
                Debug.Log("Stopping footstep sound");
                playerFootstepSource.Stop();
            }

            if (playerRunFootstepSource.clip != null && !playerRunFootstepSource.isPlaying &&
            InputManager.GetInstance().IsSprinting)
            {
                Debug.Log("Playing running footstep sound");
                playerRunFootstepSource.Play();
            }
            else if (playerRunFootstepSource.isPlaying && !InputManager.GetInstance().IsSprinting)
            {
                Debug.Log("Stopping running footstep sound");
                playerRunFootstepSource.Stop();
            }

            if (playerHeartbeatSource.clip != null && !playerHeartbeatSource.isPlaying
            && GameManager.instance.PlayerIsBeingChased)
            {
                Debug.Log("Playing heartbeat sound");
                playerHeartbeatSource.Play();
            }
            else if (playerHeartbeatSource.isPlaying && !GameManager.instance.PlayerIsBeingChased)
            {
                Debug.Log("Stopping heartbeat sound");
                playerHeartbeatSource.Stop();
            }
        }
    }
}
