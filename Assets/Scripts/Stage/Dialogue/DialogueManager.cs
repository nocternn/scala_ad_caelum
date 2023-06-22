using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("JSON")]
    [SerializeField] private string _file;
    [SerializeField] private int _iterationID;
    [SerializeField] DialogueIteration _iteration;

    public void Initialize()
    {
        _file = Application.dataPath + "/Data/Dialogues/iteration_1.json";
    }

    public void ReadDialogues()
    {
        if (!File.Exists(_file))
        {
            Debug.Log("No dialogue file found");
            return;
        }

        using (StreamReader reader = new StreamReader(_file))
        {
            _iteration = JsonUtility.FromJson<DialogueIteration>(reader.ReadToEnd());
            Debug.Log("Read dialogues");
        }
    }

    public Queue<Dialogue> GetDialogues(int id)
    {
        List<Dialogue> currentStage;
        switch (id)
        {
            case 1:
                currentStage = _iteration.stage01;
                break;
            case 2:
                currentStage = _iteration.stage02;
                break;
            case 3:
                currentStage = _iteration.stage03;
                break;
            case 4:
                currentStage = _iteration.stage04;
                break;
            case 5:
                currentStage = _iteration.stage05;
                break;
            default:
                currentStage = _iteration.stageEnd;
                break;
        }
        return new Queue<Dialogue>(currentStage);
    }
}
