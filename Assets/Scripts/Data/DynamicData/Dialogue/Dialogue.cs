using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public List<string> sentences;

    [SerializeField] private int _sentenceID = -1;

    public string GetNextSentence()
    {
        _sentenceID++;
        
        if (_sentenceID >= sentences.Count)
        {
            _sentenceID = -1;
            return null;
        }

        return sentences[_sentenceID];
    }
}
