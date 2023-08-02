using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("File Paths")]
    [SerializeField] private string _fileName;
    [SerializeField] private string _filePath;
    
    [Header("Data")]
    [SerializeField] private DialogueIteration _iteration;
    
    public void Initialize()
    {
        _fileName = $"iteration_{StatisticsManager.Instance.playerStats.progress.iteration}.json";
        _filePath = Application.dataPath + $"/Resources/Dialogue/{_fileName}";
        
        ReadDialogues();
    }

    private void ReadDialogues()
    {
        if (!File.Exists(_filePath))
        {
            Debug.Log("No dialogue file found");
            return;
        }
        
        _iteration = JsonUtility.FromJson<DialogueIteration>(
            Resources.Load<TextAsset>("Dialogue/" + _fileName.Substring(0, _fileName.Length - 5)).ToString());
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
