using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private HUDViewStage _manager;

    [Header("UI")]
    [SerializeField] private TMP_Text _txtName;
    [SerializeField] private TMP_Text _txtSentence;

    [Header("Data")]
    [SerializeField] private Queue<Dialogue> _dialogues;
    [SerializeField] private Dialogue _currentDialogue;

    void Awake()
    {
        _txtName     = transform.Find("Name").GetComponent<TMP_Text>();
        _txtSentence = transform.Find("Sentence").GetComponent<TMP_Text>();
    }

    public void SetManager(HUDViewStage manager)
    {
        _manager = manager;
    }

    public void SetDialogues(Queue<Dialogue> dialogues)
    {
        _dialogues = dialogues;
    }

    public void StartDialogue()
    {
        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentDialogue = _dialogues.Dequeue();
        
        _txtName.text = _currentDialogue.name;
        
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
            _manager.SwitchToNextScreen();
        }
        else
        {
            StartDialogue();
        }
    }
}
