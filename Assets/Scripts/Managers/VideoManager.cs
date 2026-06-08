using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance;
    [Header("Video Player Reference")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string sceneToLoadAfterVideo;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance found. Destroying this instance.");
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }

        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void Update()
    {
        // ONLY FOR DEBUGGING PURPOSES
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SkipVideo();
        }
    }

    void SkipVideo()
    {
        Debug.Log("Skipping video.");
        videoPlayer.Stop();
        GameSceneManager.instance.MoveToScene(sceneToLoadAfterVideo);
    }

    void OnPrepareCompleted(VideoPlayer vp) 
    {
        Debug.Log("Video prepared. Starting playback.");
        // vp.Play();
    }

    void OnLoopPointReached(VideoPlayer vp)
    {
        Debug.Log("Video finished. Ending playback.");
        vp.Stop();
        GameSceneManager.instance.MoveToScene(sceneToLoadAfterVideo);
    }

}
