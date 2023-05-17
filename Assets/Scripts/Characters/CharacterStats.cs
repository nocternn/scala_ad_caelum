using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
	[Header("Base Stats")]
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseCritical;
    
    [Header("Health Stats")]
    public int currentHealth;
    public int maxHealth;
    
    [Header("Stats Scale")]
    public float scaleAttack = 1;
    public float scaleCritical = 1;
    public float scaleDefense = 1;
    public float scaleHealth = 1;

	private int _critChance = 0;
	
	public void CalculateCritChance(int stageID)
    {
		_critChance = (baseCritical / (stageID * 5 + 75)) * 100;
    }

	public int GetOutgoingDamage(int atk)
	{
		int currentDamage = baseAttack + atk;
		
		if (Random.Range(1, 100) <= _critChance)
		{
			scaleCritical = 2;
		}
		else
		{
			scaleCritical = 1;
		}
		float critMultiplier = (1 + scaleAttack) * scaleCritical;

		return ScaleStat(currentDamage, critMultiplier);
	}

	public int GetIncomingDamage(int baseDamage)
	{
		int currentDamage = baseDamage;
		float damageMultiplier = 100.0f / (baseDefense * scaleDefense);

		return ScaleStat(currentDamage, damageMultiplier);
	}

	public int ScaleStat(int currentStat, float scale)
	{
		return (int)Mathf.Round(currentStat * scale);
	}
}
