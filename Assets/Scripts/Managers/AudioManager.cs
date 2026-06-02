using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] bool PlayMusic;
    [SerializeField] private AudioSource backgroundMusicSource = null;
    [SerializeField] private AudioSource ambienceMusicSource = null;
    [SerializeField] private AudioSource playerFootstepSource = null;
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
            && InputManager.GetInstance().IsMoving)
            {
                Debug.Log("Playing footstep sound");
                playerFootstepSource.Play();
            }
            else if (playerFootstepSource.isPlaying && !InputManager.GetInstance().IsMoving)
            {
                Debug.Log("Stopping footstep sound");
                playerFootstepSource.Stop();
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
