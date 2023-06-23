using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectBodhi : CardItemEffect
{
    public override void Apply(CardItem card)
    {
        switch (card.id)
        {
            case 1:
	            if (canBeApplied_01) MottoOfPubbeNivasanussati(card);
                break;
			case 2:
				if (canBeApplied_02) MottoOfDibbaSota(card);
				break;
			case 3:
				if (canBeApplied_03) MottoOfAsavakkhaya(card);
				break;
			default:
				UnityEngine.Debug.Log("Invalid Bodhi card ID");
				break;
        }
    }

    private void MottoOfPubbeNivasanussati(CardItem card)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        if (PlayerManager.Instance.stats.hitCount < (int)Mathf.Round(count))
            return;
		
        canBeApplied_01 = false;

        PlayerManager.Instance.stats.scaleAttack += scalar / 100.0f;

        Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
		{
			PlayerManager.Instance.stats.hitCount = 0;
			PlayerManager.Instance.stats.scaleAttack -= scalar / 100.0f;
			canBeApplied_01 = true;
		});
    }

	private void MottoOfDibbaSota(CardItem card)
	{
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        if (PlayerManager.Instance.stats.hitCount < (int)Mathf.Round(count))
            return;

		canBeApplied_02 = false;

        PlayerManager.Instance.stats.scaleDefense += scalar / 100.0f;

        Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
			PlayerManager.Instance.stats.hitCount = 0;
			PlayerManager.Instance.stats.scaleDefense -= scalar / 100.0f;
			canBeApplied_02 = true;
		});
	}

	private void MottoOfAsavakkhaya(CardItem card)
	{
		float count = card.count[card.level];
        float duration = card.duration[card.level];

		canBeApplied_03 = false;

		PlayerManager.Instance.stats.hitCount += (int)Mathf.Round(count);

		Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
			canBeApplied_03 = true;
		});
	}
}
