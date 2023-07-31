using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatisticsMeta
{
    public int numberOfLocalBattlesWon;
    public int numberOfStagesCleared;
    public int numberOfRuns;
    public int numberOfDeaths;
    public int totalDamageDealt;
    public int totalDamageReceived;
    public int maxDamageSingleHit;

    public void CopyFrom(StatisticsMeta meta)
    {
        numberOfLocalBattlesWon = meta.numberOfLocalBattlesWon;
        numberOfStagesCleared = meta.numberOfStagesCleared;
        numberOfRuns = meta.numberOfRuns;
        numberOfDeaths = meta.numberOfDeaths;
        totalDamageDealt = meta.totalDamageDealt;
        totalDamageReceived = meta.totalDamageReceived;
        maxDamageSingleHit = meta.maxDamageSingleHit;
    }
}
