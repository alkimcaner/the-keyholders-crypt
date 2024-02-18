using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public GameObject publicDialogueObject;
    public TMP_Text publicDialogueText;
    static GameObject dialogueObject;
    static TMP_Text dialogueText;
    static string[] lines = {
        "I need more gold.",
        "I wonder what's behind that door...",
        "I found these keys at the barn. I can sell them to you if you want.",
        "HAHA! I fooled you! You've paid a lot gold for those keys. Now it's time to pay with your life.",
        "NOOOOO!"
        };

    void Start()
    {
        dialogueObject = publicDialogueObject;
        dialogueText = publicDialogueText;
    }

    public static IEnumerator ShowDialogue(int lineIndex)
    {
        if (!dialogueObject.activeInHierarchy)
        {
            dialogueText.text = lines[lineIndex];
            dialogueObject.SetActive(true);
            yield return new WaitForSeconds(3);
            dialogueObject.SetActive(false);
        }
    }
}
