using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string _name;
    [SerializeField] private Queue<string> _sentences;

    public Dialogue(string name)
    {
        _name = name;
    }

    public void Add(string sentence)
    {
        _sentences.Enqueue(sentence);
    }

    public string GetSpeaker()
    {
        return _name;
    }
    public string GetNextSentence()
    {
        if (_sentences.Count == 0)
            return null;
        return _sentences.Dequeue();
    }
}
