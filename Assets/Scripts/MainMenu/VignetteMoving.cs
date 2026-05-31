using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimationVignette : MonoBehaviour
{
    [Header("Animation Frames")]
    [SerializeField] private Sprite[] vignetteFrames;
    [SerializeField] private float framesPerSecond = 10f;
    [SerializeField] private bool loop = true;

    [Header("Color Controls")]
    // Changing this will instantly update the UI Image in the editor!
    [SerializeField] private Color vignetteColor = Color.black; 

    private Image uiImage;
    private int currentFrameIndex;
    private float timer;
    private float timePerFrame;

    void Start()
    {
        uiImage = GetComponent<Image>();
        
        if (vignetteFrames == null || vignetteFrames.Length == 0)
        {
            Debug.LogError("Please assign some vignette sprites to the array!", this);
            enabled = false;
            return;
        }

        timePerFrame = 1f / framesPerSecond;
        
        // Apply the runtime color and initial sprite
        uiImage.sprite = vignetteFrames[0];
        uiImage.color = vignetteColor;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timePerFrame)
        {
            timer -= timePerFrame;
            AdvanceFrame();
        }
    }

    private void AdvanceFrame()
    {
        currentFrameIndex++;

        if (currentFrameIndex >= vignetteFrames.Length)
        {
            if (loop)
            {
                currentFrameIndex = 0;
            }
            else
            {
                currentFrameIndex = vignetteFrames.Length - 1;
                enabled = false;
                return;
            }
        }

        uiImage.sprite = vignetteFrames[currentFrameIndex];
    }

    // This runs automatically in the Unity Editor whenever you change a value in the Inspector
    private void OnValidate()
    {
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }
        if (uiImage != null)
        {
            uiImage.color = vignetteColor;
        }
    }
}