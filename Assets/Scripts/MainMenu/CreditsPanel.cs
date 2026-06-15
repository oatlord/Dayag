using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsPanel : Menu, IPointerClickHandler
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private CanvasGroup creditsCanvasGroup;

    private void Awake()
    {
        if (creditsCanvasGroup == null)
            creditsCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mainMenu != null)
        {
            mainMenu.ActivateMenu();
            mainMenu.EnableMenuGraphics();
        }

        SetCanvasGroupVisibility(creditsCanvasGroup, false);
    }

    private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
    {
        if (group == null) return;
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}
