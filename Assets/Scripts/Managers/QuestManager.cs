using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Serializable] public struct Quest
    {
        public string nameOfLinkedBoolean;
        public TextMeshProUGUI questCompleteCounter;
        public TextMeshProUGUI questText;
    }
    [SerializeField] private Quest quest;

    void Update()
    {
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
