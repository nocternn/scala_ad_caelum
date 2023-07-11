using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("File Paths")]
    [SerializeField] private string _fileProgress;
    [SerializeField] private string _fileDialogue;
    
    [Header("Data")]
    [SerializeField] private DialogueIteration _iteration;
    [SerializeField] private DialogueIterationProgress _iterationProgress;

    private void Awake()
    {
        _fileProgress = Application.dataPath + "/Data/JSON/progress.json";
    }
    
    public void Initialize()
    {
        ReadProgress();
        ReadDialogues();
    }
    
    private void ReadProgress()
    {
        if (!File.Exists(_fileProgress))
        {
            Debug.Log("No progress file found for reading");
            return;
        }

        using (StreamReader reader = new StreamReader(_fileProgress))
        {
            JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), _iterationProgress);

            _fileDialogue = Application.dataPath +
                            $"/Data/JSON/Dialogue/iteration_{_iterationProgress.currentIteration}.json";
        }
    }
    public void WriteProgress()
    {
        if (!File.Exists(_fileProgress))
        {
            Debug.Log("No progress file found for writing");
            return;
        }
        
        using (StreamWriter writer = new StreamWriter(_fileProgress))
        {
            _iterationProgress.currentIteration++;
            if (_iterationProgress.currentIteration > 5)
            {
                _iterationProgress.currentIteration = 1;
            }
            
            writer.Write(JsonUtility.ToJson(_iterationProgress));
        }
    }

    private void ReadDialogues()
    {
        if (!File.Exists(_fileDialogue))
        {
            Debug.Log("No dialogue file found");
            return;
        }

        using (StreamReader reader = new StreamReader(_fileDialogue))
        { 
            JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), _iteration);
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
