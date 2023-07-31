using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statistics
{
    public string name;
    public string weapon;
    public StatisticsProgress progress;
    public StatisticsCombat combat;
    public StatisticsMeta meta;

    public void CopyFrom(Statistics stats)
    {
        name = stats.name;
        weapon = stats.weapon;
        progress.CopyFrom(stats.progress);
        combat.CopyFrom(stats.combat);
        meta.CopyFrom(stats.meta);
    }

    public string GetDescription()
    {
        string description = "Local battles won: " + meta.numberOfLocalBattlesWon;
        description += "\nWeapon: " + weapon;
        description += "\nHP: " + combat.ScaleStat(combat.baseHealth, combat.scaleHealth).ToString();
        description += "\nATK: " + combat.ScaleStat(combat.baseAttack, combat.scaleAttack).ToString();
        description += "\nDEF: " + combat.ScaleStat(combat.baseDefense, combat.scaleDefense).ToString();
        description += "\nCRT: " + combat.ScaleStat(combat.baseCritical, combat.scaleCritical).ToString();
        return description;
    }
}
