using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.Animations;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("CG UI")]
    [Tooltip("Can be left null if there are no CGs in this zone's dialogue.")]
    [SerializeField] private GameObject cgCanvas = null;

    [Header("Player Control Maps")]
    // Reference to the scene's currently active player control map name to automatically switch to.
    private string sceneCurrentlyActivePlayerControlMapName;
    // [SerializeField] private string playerControlMapName;
    [SerializeField] private string uiControlMapName = "UI_Input";

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private bool canContinueToNextLine = false;

    private static DialogueManager instance;
    private DialogueVariables dialogueVariables;
    private Coroutine displayLineCoroutine;

    private const string SPEAKER_TAG = "speaker";
    private const string SCENE_TRANSITION = "moveToScene";
    private const string CG_IMAGE = "cgImage";

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one of this instance exists.");
        }
        else
        {
            instance = this;
            dialogueVariables = new DialogueVariables(loadGlobalsJSON);
            // animator = player.GetComponent<Animator>();
        }
    }

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine
        && currentStory.currentChoices.Count == 0
        && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void SaveData(GameData data)
    {
        data.savedStoryJson = dialogueVariables.GetSaveStateJson();
    }

    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(data.savedStoryJson))
        {
            dialogueVariables.LoadStateJson(data.savedStoryJson);
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        // Always save the currently active map before switching to uiControlMap.
        sceneCurrentlyActivePlayerControlMapName = InputManager.GetInstance().GetCurrentlyActiveMap();
        Debug.Log(sceneCurrentlyActivePlayerControlMapName);

        if (InputManager.GetInstance().GetCurrentlyActiveMap() != uiControlMapName)
        {
            InputManager.GetInstance().SwitchToUIMap();
        }

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        dialogueVariables.StopListening(currentStory);

        // if (InputManager.GetInstance().GetCurrentlyActiveMap() != playerControlMapName)
        // {
        //     InputManager.GetInstance().SwitchToPlayerMap(playerControlMapName);
        // }
        // Switch to the saved currently active player control map name when exiting dialogue mode.
        if (InputManager.GetInstance().GetCurrentlyActiveMap() != sceneCurrentlyActivePlayerControlMapName)
        {
            InputManager.GetInstance().SwitchToPlayerMap(sceneCurrentlyActivePlayerControlMapName);
            sceneCurrentlyActivePlayerControlMapName = "";
        }

        if (cgCanvas != null && cgCanvas.activeSelf)
        {
            cgCanvas.SetActive(false);
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            // dialogueText.text = currentStory.Continue();
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // clear dialogue
        dialogueText.text = "";

        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        foreach (char letter in line.ToCharArray())
        {
            if (InputManager.GetInstance().GetSubmitPressed())
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        // Hide CG canvas by default; only show when a CG_IMAGE tag is present, return all children of cgCanvas
        List<GameObject> cgImages = new List<GameObject>();
        if (cgCanvas != null)
        {
            cgCanvas.SetActive(false);
            foreach (Transform child in cgCanvas.transform)
            {
                child.gameObject.SetActive(false);
                cgImages.Add(child.gameObject);
            }
        }

        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
                continue;
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    Debug.Log("Speaker: " + tagValue);
                    if (tagValue == "Narrator")
                    {
                        displayNameText.text = "";
                    }
                    else
                    {
                        displayNameText.text = tagValue;
                    }
                    break;
                case SCENE_TRANSITION:
                    Debug.Log("Scene transition tag with value: " + tagValue);
                    GameSceneManager.instance.MoveToScene(tagValue);
                    break;
                case CG_IMAGE:
                    if (cgCanvas != null)
                    {
                        Debug.Log("Showing CG canvas.");
                        cgCanvas.SetActive(true);
                        if (int.TryParse(tagValue, out int result))
                        {
                            if (result >= 0 && result < cgImages.Count)
                            {
                                cgImages[result].SetActive(true);
                            }
                            else
                            {
                                Debug.LogWarning($"CG image index out of range: {result}. Valid range is 0 to {cgImages.Count - 1}.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"CG image tag value is not a valid index: {tagValue}");
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not being currently handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            InputManager.GetInstance().RegisterSubmitPressed();
            ContinueStory();
        }
    }

    public void SetVariableState(string variableName, Ink.Runtime.Object value)
    {
        if (dialogueVariables == null)
        {
            Debug.LogError("DialogueVariables not initialized.");
            return;
        }

        if (dialogueVariables.variables.ContainsKey(variableName))
        {
            dialogueVariables.variables[variableName] = value;
        }
        else
        {
            dialogueVariables.variables.Add(variableName, value);
        }

        if (dialogueVariables.globalVariablesStory != null)
        {
            dialogueVariables.globalVariablesStory.variablesState.SetGlobal(variableName, value);
        }

        if (currentStory != null)
        {
            currentStory.variablesState.SetGlobal(variableName, value);
        }
    }

    public void SetVariableState(string variableName, bool value)
    {
        SetVariableState(variableName, new Ink.Runtime.BoolValue(value));
    }


    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        if (dialogueVariables != null && dialogueVariables.variables.TryGetValue(variableName, out variableValue) && variableValue != null)
        {
            return variableValue;
        }

        if (currentStory != null)
        {
            variableValue = currentStory.variablesState.GetVariableWithName(variableName);
            if (variableValue != null)
            {
                return variableValue;
            }
        }

        Debug.LogError("Variable requested but not found: " + variableName);
        return null;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void OnDisable()
    {
        Debug.LogError($"[CATCHER] DialogueManager was just UNCHECKED/DISABLED!", this);
        Debug.Log(System.Environment.StackTrace);
    }

    public void OnApplicationQuit()
    {
        // dialogueVariables.SaveVariables();
    }
}
