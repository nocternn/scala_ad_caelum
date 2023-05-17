using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Card")]
public class CardItem : Item
{
	public Enums.CardType type;

    public int id;
    
    public float[] count;
    public float[] duration;
    public float[] scalar;

    public CardItemEffect effect;

    public void Initialize()
    {
		InitializeInfo();
		InitializeEffect();
    }

    public void LevelUp()
    {
        level++;
		effect.canBeApplied = true;
        InitializeInfo();
    }

	private void InitializeInfo()
	{
	    if (level == 0)
        {
            name = name.Replace("LEVEL", "");
        }
        else
        {
            name = name.Replace("LEVEL", "+" + level.ToString());
        }
        if (count.Length > 0)
            description = description.Replace("COUNT", count[level].ToString());
        if (duration.Length > 0)
            description = description.Replace("DURATION", duration[level].ToString());
        if (scalar.Length > 0)
            description = description.Replace("SCALAR", scalar[level].ToString());
	}

	private void InitializeEffect()
	{
		StageManager stage = GameObject.Find("StageManager").GetComponent<StageManager>();
		switch(type)
		{
			case Enums.CardType.Bodhi:
				effect = stage.buffHolder.AddComponent<CardEffectBodhi>() as CardEffectBodhi;
				break;
			case Enums.CardType.Decimation:
				effect = stage.buffHolder.AddComponent<CardEffectDecimation>() as CardEffectDecimation;
				break;
			case Enums.CardType.Deliverance:
				effect = stage.buffHolder.AddComponent<CardEffectDeliverance>() as CardEffectDeliverance;
				break;
			case Enums.CardType.Gold:
				effect = stage.buffHolder.AddComponent<CardEffectGold>() as CardEffectGold;
				break;
			case Enums.CardType.Vicissitude:
				effect = stage.buffHolder.AddComponent<CardEffectVicissitude>() as CardEffectVicissitude;
				break;
			default:
				break;
		}
	}
}
