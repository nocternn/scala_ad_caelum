using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatisticsCombat
{
    [Header("Base Stats")]
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseCritical;
    
    [Header("Stats Scale")]
    public float scaleAttack;
    public float scaleCritical;
    public float scaleDefense;
    public float scaleHealth;
    
    public void CopyFrom(StatisticsCombat combat)
    {
        baseHealth = combat.baseHealth;
        baseAttack = combat.baseAttack;
        baseDefense = combat.baseDefense;
        baseCritical = combat.baseCritical;
        scaleAttack = combat.scaleAttack;
        scaleCritical = combat.scaleCritical;
        scaleDefense = combat.scaleDefense;
        scaleHealth = combat.scaleHealth;
    }
}
