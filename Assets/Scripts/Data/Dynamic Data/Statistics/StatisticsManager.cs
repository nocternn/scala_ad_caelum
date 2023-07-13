using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    [Header("File Paths")]
    [SerializeField] private string _dirPath;
    [SerializeField] private List<Statistics> _allStats; // Statistics of all local players

    [Header("Data")] [SerializeField]
    private Statistics _defaultStats; // Default statistics
    public Statistics playerStats; // Statistics of current player

    public void Initialize()
    {
        _dirPath = Application.dataPath + "/Data/JSON/Statistics";
        ReadAllStats();
    }

    #region I/O

    private void ReadAllStats()
    {
        DirectoryInfo dir = new DirectoryInfo(_dirPath);
        FileInfo[] files = dir.GetFiles("*.json");
        
        foreach (var file in files)
        {
            using (StreamReader reader = new StreamReader(file.FullName))
            {
                if (file.Name.Equals("statistics_default.json"))
                {
                    JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), _defaultStats);
                }
                else
                {
                    Statistics stats = new Statistics();
                    JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), stats);
                    _allStats.Add(stats);
                }
            }
        }
    }

    public bool ReadStatsPlayer(string name)
    {
        bool readPlayer = false;
        
        foreach (var stats in _allStats)
        {
            if (stats.name.Equals(name))
            {
                readPlayer = true;
                playerStats = stats;
                break;
            }
        }
        
        return readPlayer;
    }

    public bool WriteStatsPlayer(string name)
    {
        bool writePlayer = false;
        
        string filePath = _dirPath + $"/statistics_{name}.json";
        if (!File.Exists(filePath))
            File.Create(filePath);
        
        foreach (var stats in _allStats)
        {
            if (stats.name.Equals(name))
            {
                writePlayer = true;
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(JsonUtility.ToJson(playerStats));
                }
                break;
            }
        }
        
        return writePlayer;
    }

    #endregion

    #region Modifiers

    public void CopyCombatStats(PlayerManager player)
    {
        playerStats.combat.baseHealth = player.stats.baseHealth;
        playerStats.combat.baseAttack = player.stats.baseAttack;
        playerStats.combat.baseDefense = player.stats.baseDefense;
        playerStats.combat.baseCritical = player.stats.baseCritical;
        playerStats.combat.scaleAttack = player.stats.scaleAttack;
        playerStats.combat.scaleCritical = player.stats.scaleCritical;
        playerStats.combat.scaleDefense = player.stats.scaleDefense;
        playerStats.combat.scaleHealth = player.stats.scaleHealth;
    }

    public void ResetStatsPlayer(string name, Enums.StatsType type)
    {
        foreach (var stats in _allStats)
        {
            if (stats.name.Equals(name))
            {
                switch (type)
                {
                    case Enums.StatsType.Progress:
                        playerStats.progress.CopyFrom(_defaultStats.progress);
                        break;
                    case Enums.StatsType.Combat:
                        playerStats.combat.CopyFrom(_defaultStats.combat);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion

    #region Getters

    public bool IsBeginning()
    {
        bool isFirstIteration = playerStats.progress.iteration == 1;
        bool isfirstStage = playerStats.progress.stage == 1;
        return isFirstIteration && isfirstStage;
    }

    public Statistics GetStatsPlayer(string name)
    {
        foreach (var stats in _allStats)
        {
            if (stats.name.Equals(name))
                return stats;
        }
        return null;
    }

    #endregion
}
