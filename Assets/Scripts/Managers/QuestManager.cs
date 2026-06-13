using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Serializable]
    public struct Quest
    {
        public string nameOfLinkedBoolean;
        [Header("Boolean Dependencies")]
        [Tooltip("If this objective appearing is dependent on another boolean.")]
        public bool BooleanDependent;
        [Tooltip("Name of boolean that this objective is dependent on for showing up, if it exists.")]
        public string nameOfDependentBoolean;
        public TextMeshProUGUI questCompleteCounter;
        public TextMeshProUGUI questText;
    }
    [SerializeField] private Quest quest;

    void Update()
    {
        if (quest.BooleanDependent)
        {
            if (quest.nameOfDependentBoolean != null)
            {
                quest.questText.gameObject.SetActive(false);
                quest.questCompleteCounter.gameObject.SetActive(false);

                if (((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState(quest.nameOfDependentBoolean)).value)
                {
                    quest.questText.gameObject.SetActive(true);
                    quest.questCompleteCounter.gameObject.SetActive(true);
                }
            }
        }
        // else
        // {
        //     quest.questText.gameObject.SetActive(true);
        //     quest.questCompleteCounter.gameObject.SetActive(true);
        // }

        if (((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState(quest.nameOfLinkedBoolean)).value)
        {
            quest.questCompleteCounter.text = "1/1";
        }
    }
    // [SerializeField] private TextMeshProUGUI[] questText;
    // [SerializeField] private string[] questBoolLinkName;
    // private Dictionary<string,string> questObjectivePairs;

    // void Start()
    // {
    //     foreach (TextMeshProUGUI questTextChild in questText)
    //     {
    //         foreach (string questBoolLinkChild in questBoolLinkName)
    //         {
    //             questObjectivePairs.Add(questTextChild.text,questBoolLinkChild);
    //         }
    //     }
    // }

    // void Update()
    // {

    // }
}
