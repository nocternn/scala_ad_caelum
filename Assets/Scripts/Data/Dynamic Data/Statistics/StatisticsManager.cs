using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance { get; private set; }

    [Header("File Paths")]
    [SerializeField] private string _dirPath;

    [Header("Data")]
    [SerializeField] private List<Statistics> _allStats; // Statistics of all local players
    [SerializeField] private Statistics _defaultStats; // Default statistics

    [Header("Data - Player")]
    public Statistics playerStats; // Statistics of current player
    public WeaponItem playerWeapon;

    [Header("Data - Opponent")]
    public Statistics opponentStats; // Statistics of current opponent
    public WeaponItem opponentWeapon;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            _dirPath = Application.dataPath + "/Data/JSON/Statistics";
            ReadAllStats();
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
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

    public bool ReadStatsPlayer(string name = "")
    {
        bool readPlayer = false;

        if (name.Length > 0)
        {
            foreach (var stats in _allStats)
            {
                if (stats.name.Equals(name))
                {
                    readPlayer = true;
                    playerStats = stats;
                    playerWeapon = MenuManager.Instance.inventoryMenu.GetWeapon(playerStats.weapon);
                    break;
                }
            }
        }
        else
        {
            readPlayer = true;
            playerStats.CopyFrom(_defaultStats);
            playerWeapon = MenuManager.Instance.inventoryMenu.GetWeapon(playerStats.weapon);
        }

        return readPlayer;
    }

    public bool WriteStatsPlayer()
    {
        bool writePlayer = false;

        if (playerStats != null)
        {
            writePlayer = true;

            string filePath = _dirPath + $"/statistics_{playerStats.name}.json";
            if (!File.Exists(filePath))         // If statistics for this player does not exist
                File.Create(filePath).Close();  // Create empty file

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(JsonUtility.ToJson(playerStats));
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

    public void ResetStatsPlayer(Enums.StatsType type)
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

    public Statistics[] GetAllStats()
    {
        return _allStats.ToArray();
    }

    #endregion
}
