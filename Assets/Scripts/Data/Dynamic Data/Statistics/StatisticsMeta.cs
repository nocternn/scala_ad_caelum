using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatisticsMeta
{
    public int numberOfRuns;
    public int numberOfStagesCleared;
    public int numberOfDeaths;
    public int totalDamageDealt;
    public int totalDamageReceived;
    public int maxDamageSingleHit;

    public void CopyFrom(StatisticsMeta meta)
    {
        numberOfRuns = meta.numberOfRuns;
        numberOfStagesCleared = meta.numberOfStagesCleared;
        numberOfDeaths = meta.numberOfDeaths;
        totalDamageDealt = meta.totalDamageDealt;
        totalDamageReceived = meta.totalDamageReceived;
        maxDamageSingleHit = meta.maxDamageSingleHit;
    }
}
