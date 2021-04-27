using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    public void Interact()
    {
        DialogueManager.Instance.ShowDlg(dialogue);
    }

    public void Close()
    {
        DialogueManager.Instance.CloseDlg();
    }
}
