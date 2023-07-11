using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    [Header("File Paths")]
    [SerializeField] private string _fileStatistics;
    
    [Header("Data")]
    public Statistics stats;

    private void Awake()
    {
        _fileStatistics = Application.dataPath + "/Data/JSON/statistics.json";
    }
    
    public bool ReadStats()
    {
        if (!File.Exists(_fileStatistics))
        {
            Debug.Log("No statistics file found for reading");
            return false;
        }

        using (StreamReader reader = new StreamReader(_fileStatistics))
        {
            JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), stats);
        }
        return true;
    }

    public bool WriteStats()
    {
        if (!File.Exists(_fileStatistics))
        {
            Debug.Log("No statistics file found for writing");
            return false;
        }
        
        using (StreamWriter writer = new StreamWriter(_fileStatistics))
        {
            writer.Write(JsonUtility.ToJson(stats));
        }
        return true;
    }
}
