using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingScreenTypingEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingStatusText;
    [SerializeField] private string baseText = "Loading";
    [SerializeField] private float dotSpeed = 0.25f;
    [SerializeField] private int maxDots = 3;

    private Coroutine loadingCoroutine;

    private void Awake()
    {
        if (loadingStatusText == null)
            return;

        loadingCoroutine = StartCoroutine(LoadingAnimation());
    }

    private IEnumerator LoadingAnimation()
    {
        int dotCount = 0;

        while (true)
        {
            loadingStatusText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % (maxDots + 1);
            yield return new WaitForSeconds(dotSpeed);
        }
    }
}
