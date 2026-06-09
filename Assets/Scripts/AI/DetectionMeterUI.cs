using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this to the same GameObject as AIController (or a child).
/// It creates a world-space Canvas above the NPC's head with a Slider
/// that visualises how close the enemy is to fully detecting the player.
///
/// The meter:
///   - Is hidden when detectionProgress == 0
///   - Fills (yellow → red) as detectionProgress approaches 1
///   - Flashes red briefly when the enemy starts chasing
///   - Always faces the main camera (billboard effect)
/// </summary>
[RequireComponent(typeof(AIController))]
public class DetectionMeterUI : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────
    [Header("Meter Position")]
    [Tooltip("How far above the NPC's pivot the meter floats (metres).")]
    [SerializeField] private float heightOffset = 2.4f;

    [Header("Meter Size")]
    [SerializeField] private float meterWidth  = 1.2f;   // world-space width
    [SerializeField] private float meterHeight = 0.18f;  // world-space height

    [Header("Colors")]
    [SerializeField] private Color colorLow    = new Color(1f, 0.85f, 0f);   // yellow
    [SerializeField] private Color colorHigh   = new Color(1f, 0.15f, 0f);   // red
    [SerializeField] private Color bgColor     = new Color(0f, 0f, 0f, 0.55f);
    [SerializeField] private Color chaseFlash  = new Color(1f, 0f, 0f, 1f);

    [Header("Behaviour")]
    [Tooltip("Seconds the red flash lasts when the enemy locks on.")]
    [SerializeField] private float flashDuration = 0.35f;
    [Tooltip("How quickly the canvas fades in/out (alpha per second).")]
    [SerializeField] private float fadeSpped = 4f;

    // ── Runtime references ────────────────────────────────────────────────────
    private AIController    ai;
    private Camera          mainCam;
    private Canvas          canvas;
    private CanvasGroup     canvasGroup;
    private Slider          slider;
    private Image           fillImage;
    private Image           bgImage;

    private float  flashTimer   = 0f;
    private bool   wasChasing   = false;
    private float  targetAlpha  = 0f;

    // Once fully detected, hide the meter until the AI resets
    private bool   lockedOut    = false;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        ai      = GetComponent<AIController>();
        mainCam = Camera.main;

        BuildUI();
    }

    void Update()
    {
        UpdateVisibility();
        UpdateFill();
        UpdateFlash();
        BillboardCanvas();
    }

    // ── UI Construction ───────────────────────────────────────────────────────

    /// <summary>
    /// Procedurally builds a world-space Canvas → background Image → Slider
    /// so there's nothing to drag-and-drop in the inspector.
    /// </summary>
    private void BuildUI()
    {
        // ── Canvas ──
        GameObject canvasGO = new GameObject("DetectionMeter_Canvas");
        canvasGO.transform.SetParent(transform, false);
        canvasGO.transform.localPosition = new Vector3(0f, heightOffset, 0f);

        canvas             = canvasGO.AddComponent<Canvas>();
        canvas.renderMode  = RenderMode.WorldSpace;

        canvasGroup        = canvasGO.AddComponent<CanvasGroup>();
        canvasGroup.alpha  = 0f;

        // Size the canvas to match world-space dimensions
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta      = new Vector2(meterWidth * 100f, meterHeight * 100f);
        canvasRect.localScale     = new Vector3(0.01f, 0.01f, 0.01f);  // 1 unit = 100 px

        // ── Background ──
        GameObject bgGO  = new GameObject("Background");
        bgGO.transform.SetParent(canvasGO.transform, false);
        bgImage          = bgGO.AddComponent<Image>();
        bgImage.color    = bgColor;

        RectTransform bgRect    = bgImage.rectTransform;
        bgRect.anchorMin        = Vector2.zero;
        bgRect.anchorMax        = Vector2.one;
        bgRect.offsetMin        = Vector2.zero;
        bgRect.offsetMax        = Vector2.zero;

        // ── Slider ──
        GameObject sliderGO = new GameObject("DetectionSlider");
        sliderGO.transform.SetParent(canvasGO.transform, false);
        slider = sliderGO.AddComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value    = 0f;
        slider.interactable = false;   // player can't touch it

        RectTransform sliderRect = slider.GetComponent<RectTransform>();
        sliderRect.anchorMin  = new Vector2(0.02f, 0.1f);
        sliderRect.anchorMax  = new Vector2(0.98f, 0.9f);
        sliderRect.offsetMin  = Vector2.zero;
        sliderRect.offsetMax  = Vector2.zero;

        // Slider background (transparent — outer bg handles it)
        GameObject sliderBg = new GameObject("SliderBackground");
        sliderBg.transform.SetParent(sliderGO.transform, false);
        Image sliderBgImg         = sliderBg.AddComponent<Image>();
        sliderBgImg.color         = new Color(0f, 0f, 0f, 0f);
        RectTransform sbRect      = sliderBgImg.rectTransform;
        sbRect.anchorMin          = Vector2.zero;
        sbRect.anchorMax          = Vector2.one;
        sbRect.offsetMin          = Vector2.zero;
        sbRect.offsetMax          = Vector2.zero;

        // Fill area
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderGO.transform, false);
        RectTransform faRect = fillArea.AddComponent<RectTransform>();
        faRect.anchorMin = Vector2.zero;
        faRect.anchorMax = Vector2.one;
        faRect.offsetMin = Vector2.zero;
        faRect.offsetMax = Vector2.zero;

        // Fill image
        GameObject fillGO = new GameObject("Fill");
        fillGO.transform.SetParent(fillArea.transform, false);
        fillImage         = fillGO.AddComponent<Image>();
        fillImage.color   = colorLow;

        RectTransform fillRect = fillImage.rectTransform;
        fillRect.anchorMin     = Vector2.zero;
        fillRect.anchorMax     = Vector2.one;
        fillRect.offsetMin     = Vector2.zero;
        fillRect.offsetMax     = Vector2.zero;

        // Wire fill into slider
        slider.fillRect      = fillRect;
        slider.targetGraphic = sliderBgImg;
    }

    // ── Per-Frame Logic ───────────────────────────────────────────────────────

    private void UpdateVisibility()
    {
        // When the meter hits full, lock it out (hide) until the AI resets
        if (!lockedOut && ai.detectionProgress >= 1f)
        {
            lockedOut = true;
        }

        // Unlock when the AI resets back to 0 (e.g. post-player-death)
        if (lockedOut && ai.detectionProgress <= 0f)
        {
            lockedOut = false;
        }

        // Show only while there's progress AND not locked out
        targetAlpha = (!lockedOut && ai.detectionProgress > 0.01f) ? 1f : 0f;
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpped * Time.deltaTime);
    }

    private void UpdateFill()
    {
        slider.value = ai.detectionProgress;

        // Lerp colour from yellow → red as progress rises
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(colorLow, colorHigh, ai.detectionProgress);
        }
    }

    private void UpdateFlash()
    {
        bool isChasing = ai.detectionProgress >= 1f;

        // Trigger flash the moment chasing starts
        if (isChasing && !wasChasing)
        {
            flashTimer = flashDuration;
        }
        wasChasing = isChasing;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            float t = flashTimer / flashDuration;
            // Pulse between chaseFlash and normal fill colour
            fillImage.color = Color.Lerp(Color.Lerp(colorLow, colorHigh, 1f), chaseFlash, t);
        }
    }

    /// <summary>Keep the canvas facing the camera every frame.</summary>
    private void BillboardCanvas()
    {
        if (canvas == null || mainCam == null) return;
        canvas.transform.LookAt(
            canvas.transform.position + mainCam.transform.rotation * Vector3.forward,
            mainCam.transform.rotation * Vector3.up
        );
    }
}