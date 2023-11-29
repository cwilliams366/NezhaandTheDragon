using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour
{
    [Header("Visual Cue Data")]
    public GameObject visualCue;
    private bool IsPlayerClose = false;

    [Header("Text Info")]
    public TextAsset inkFile;

    private void Awake()
    {
        visualCue.SetActive(false);
    }


    private void Update()
    {
        if (IsPlayerClose && !DialogueManagerScript.instance.IsDialoguePlaying)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DialogueManagerScript.instance.EnterDialogueMode(inkFile);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsPlayerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsPlayerClose = true;
        }
    }
}
