using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Card")]
public class CardItem : Item
{
	public bool canUpgrade;

	public Enums.CardType type;

    public int id;
    public int level;

    public int cost;
    public string costDescription;
    
    public float[] count;
    public float[] duration;
    public float[] scalar;

    public CardItemEffect effect;

    public void Initialize()
    {
	    level = 0;
	    canUpgrade = true;
	    GetLevelUpCondition(); // Set initial cost value
    }

    public void EnableEffect(GameObject holder)
    {
	    switch(type)
	    {
		    case Enums.CardType.Bodhi:
			    effect = holder.AddComponent<CardEffectBodhi>() as CardEffectBodhi;
			    break;
		    case Enums.CardType.Decimation:
			    effect = holder.AddComponent<CardEffectDecimation>() as CardEffectDecimation;
			    break;
		    case Enums.CardType.Deliverance:
			    effect = holder.AddComponent<CardEffectDeliverance>() as CardEffectDeliverance;
			    break;
		    case Enums.CardType.Gold:
			    effect = holder.AddComponent<CardEffectGold>() as CardEffectGold;
			    break;
		    case Enums.CardType.Vicissitude:
			    effect = holder.AddComponent<CardEffectVicissitude>() as CardEffectVicissitude;
			    break;
		    default:
			    break;
	    }
    }

    public void LevelUp()
    {
	    if (canUpgrade)
	    {
		    level++;
		    GetLevelUpCondition(); // Set initial cost value

		    switch (id)
		    {
			    case 1:
				    effect.canBeApplied_01 = true;
				    break;
			    case 2:
				    effect.canBeApplied_02 = true;
				    break;
			    case 3:
				    effect.canBeApplied_03 = true;
				    break;
		    }
	    }

	    if (level == 3)
		    canUpgrade = false;
    }

	public string GetName()
	{
		string updatedName = name;
	    if (level == 0)
        {
            updatedName = updatedName.Replace("LEVEL", "");
        }
		else
		{
        	updatedName = updatedName.Replace("LEVEL", "+" + level.ToString());
		}
		return updatedName;
	}

	public string GetDescription()
	{
		string updatedDescription = description;
        if (count.Length > 0)
            updatedDescription = updatedDescription.Replace("COUNT", count[level].ToString());
        if (duration.Length > 0)
            updatedDescription = updatedDescription.Replace("DURATION", duration[level].ToString());
        if (scalar.Length > 0)
            updatedDescription = updatedDescription.Replace("SCALAR", scalar[level].ToString());
		return updatedDescription;
	}

	public string GetCost()
	{
		string updatedCost = "Cost: COST";
		if (cost == 0)
		{
			updatedCost = updatedCost.Replace("COST", "MAX");
		}
		else
		{
			updatedCost = updatedCost.Replace("COST", cost.ToString());
		}
		return updatedCost;
	}

	public int GetLevelUpCondition()
	{
		switch (level)
		{
			case 0:
				cost =  80;
				break;
			case 1:
				cost = 120;
				break;
			case 2:
				cost = 160;
				break;
			default:
				cost = 0;
				break;
		}
		return cost;
	}
}
