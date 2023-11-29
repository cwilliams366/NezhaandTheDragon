using Ink.Runtime;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManagerScript : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Available Choices UI")]
    public GameObject[] dialogueChoices;
    private TextMeshProUGUI[] dialogueTextChoices; 

    private Story currentStory;
    public bool IsDialoguePlaying { get; private set; }
    public static DialogueManagerScript instance { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);

        dialogueTextChoices = new TextMeshProUGUI[dialogueChoices.Length];
        int i = 0;
        foreach (GameObject option in dialogueChoices)
        {
            dialogueTextChoices[i] = option.GetComponentInChildren<TextMeshProUGUI>();
            i++;
        }

    }

    private void Update()
    {
        if (!IsDialoguePlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkFile)
    {
        currentStory = new Story(inkFile.text);
        IsDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

    }

    private IEnumerator ExitingDialogueMode()
    {
        yield return new WaitForSeconds(0.3f);
        IsDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            //Display Options
            ShowChoices();
        }
        else
        {
            StartCoroutine(ExitingDialogueMode());
        }
    }

    private void ShowChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > dialogueChoices.Length)
        {
            Debug.Log("More choices were given than the UI can possibly support");
        }

        int i = 0;
        foreach(Choice choice in currentChoices)
        {
            dialogueChoices[i].gameObject.SetActive(true);
            dialogueTextChoices[i].text = choice.text;
            i++;

        }

        for(int x = i; x < dialogueChoices.Length; x++)
        {
            dialogueChoices[x].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(dialogueChoices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
