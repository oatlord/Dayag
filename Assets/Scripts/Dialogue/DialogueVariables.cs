using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    public Story globalVariablesStory;
    // private GameData data;
    // private const string saveVariablesKey = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        // if (PlayerPrefs.HasKey(saveVariablesKey))
        // {
        //     string json = PlayerPrefs.GetString(saveVariablesKey);
        //     globalVariablesStory.state.LoadJson(json);
        // }

        // if (GameData data.savedStoryJson != "")
        // {
        //     globalVariablesStory.state.LoadJson(data.savedStoryJson);
        // }

        // if (data.savedStoryJson != "")
        // {
        //     globalVariablesStory.state.LoadJson(data.savedStoryJson);
        // }

        // initialize dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global variable: " + name + " = value: " + value);
        }
    }

    public void SaveVariables()
    {
        if (globalVariablesStory != null)
        {
            // Load current state of all variables to the globals
            VariablesToStory(globalVariablesStory);
        }
    }

    public string GetSaveStateJson()
    {
        SaveVariables();
        return globalVariablesStory != null ? globalVariablesStory.state.ToJson() : string.Empty;
    }

    public void LoadStateJson(string json)
    {
        if (globalVariablesStory == null || string.IsNullOrEmpty(json))
        {
            return;
        }

        globalVariablesStory.state.LoadJson(json);
        RefreshVariablesFromGlobals();
    }

    private void RefreshVariablesFromGlobals()
    {
        if (globalVariablesStory == null)
        {
            return;
        }

        variables.Clear();
        foreach (string name in globalVariablesStory.variablesState)
        {
            variables[name] = globalVariablesStory.variablesState.GetVariableWithName(name);
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
        
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        // Debug.Log($"Variable changed: {name} - new value: {value}");

        // only maintain variables initialized from the globals ink file
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
