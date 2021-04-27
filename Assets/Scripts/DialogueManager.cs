using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] GameObject Wizard;
    public static DialogueManager Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDlg(Dialogue dialogue)
    {
        Wizard.GetComponent<Movement>().enabled = false;
        Wizard.GetComponent<Movement>().animator.SetBool("isMoving", false);
        dialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[0], dialogue));
        //dialogueText.TextMeshProUGUI = dialogue.Lines[0];
    }

    public void CloseDlg()
    {
        dialogueBox.SetActive(false);
        Wizard.GetComponent<Movement>().enabled = true;
        Wizard.GetComponent<Movement>().animator.SetBool("isMoving", true);
    }

    public IEnumerator TypeDialogue(string line, Dialogue dialogue)
    {
        dialogueText.text = "";
        int c = -1;
        foreach (var l in dialogue.Lines)
        {
            ++c;
            foreach (var letter in dialogue.Lines[c].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(1f/ lettersPerSecond);
            }
            new WaitForSeconds(10f);
            if(dialogue.Lines.Count != c-1)
            {
                dialogueText.text = "";
            }
        }
        CloseDlg();
    }
}
