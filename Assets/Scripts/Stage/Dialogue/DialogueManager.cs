using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private StageManager _stage;
    
    [Header("Data")]
    [SerializeField] private Queue<Dialogue> _dialogues;
    [SerializeField] private Dialogue _currentDialogue;

    [Header("UI")]
    [SerializeField] private TMP_Text _txtName;
    [SerializeField] private TMP_Text _txtSentence;

    public void Initialize()
    {
        _dialogues = new Queue<Dialogue>();

        _txtName     = transform.Find("Name").GetComponent<TMP_Text>();
        _txtSentence = transform.Find("Sentence").GetComponent<TMP_Text>();

        ReadDialogues();
    }

    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }

    public void StartDialogue()
    {
        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentDialogue = _dialogues.Dequeue();
        
        _txtName.text = _currentDialogue.GetSpeaker();
        
        ContinueDialogue();
    }
    
    public void ContinueDialogue()
    {
        _txtSentence.text = _currentDialogue.GetNextSentence();
        if (_txtSentence.text == null)
            EndDialogue();
    }
    
    public void EndDialogue()
    {
        if (_dialogues.Count == 0)
        {
            _stage.SwitchToBuff();
            this.gameObject.SetActive(false);
        }
        else
        {
            StartDialogue();
        }
    }

    private void ReadDialogues()
    {
        _dialogues.Clear();
    }
}
