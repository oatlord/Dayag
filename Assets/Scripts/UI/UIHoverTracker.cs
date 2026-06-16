using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIHoverTracker : MonoBehaviour
{
    [SerializeField] private GameObject currentlyHoveredObject;

    void Update()
    {
        currentlyHoveredObject = GetHoveredUIElement();
        
        if (currentlyHoveredObject != null)
        {
            // Do something with the hovered object
            Debug.Log($"Currently hovering over: {currentlyHoveredObject.name}");
        }
    }

    private GameObject GetHoveredUIElement()
    {
        // 1. Setup a fake pointer event using the current mouse/pointer position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        
        // Use New Input System syntax to grab pointer position safely
        if (Mouse.current != null)
        {
            eventData.position = Mouse.current.position.ReadValue();
        }
        else if (Pointer.current != null)
        {
            eventData.position = Pointer.current.position.ReadValue();
        }
        else
        {
            return null;
        }

        // 2. Raycast against the UI Canvas Graphic Raycasters
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 3. Return the first hit UI element (if any)
        if (results.Count > 0)
        {
            return results[0].gameObject;
        }

        return null;
    }
}
