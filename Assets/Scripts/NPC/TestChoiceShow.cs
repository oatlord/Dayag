using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoiceShow : MonoBehaviour
{
    // THIS IS ONLY FOR TESTING PURPOSES FOR REFERENCING THE VARIABLES.
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color choice1Color = Color.red;
    [SerializeField] private Color choice2Color = Color.blue;

    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        string choiceName = ((Ink.Runtime.StringValue) DialogueManager.GetInstance().GetVariableState("NameOfChoice")).value;

        switch (choiceName)
        {
            case "":
                objectRenderer.material.color = defaultColor;
                break;
            case "Choice 1":
                objectRenderer.material.color = choice1Color;
                break;
            case "Choice 2":
                objectRenderer.material.color = choice2Color;
                break;
            default:
                Debug.LogWarning($"Unexpected choice name: {choiceName}");
                break;
        }
    }
}
