using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    public Canvas TextboxUI;
    public TextMeshProUGUI npcNameLabel;

    public void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
        TextboxUI.gameObject.SetActive(true);
    }
}
