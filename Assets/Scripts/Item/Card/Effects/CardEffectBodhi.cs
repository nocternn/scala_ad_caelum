using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffectBodhi : CardItemEffect
{
    public override void Apply(CardItem card, PlayerManager player = null, EnemyManager enemy = null)
    {
        switch (card.id)
        {
            case 1:
	            if (canBeApplied_01) MottoOfPubbeNivasanussati(card, player);
                break;
			case 2:
				if (canBeApplied_02) MottoOfDibbaSota(card, player);
				break;
			case 3:
				if (canBeApplied_03) MottoOfAsavakkhaya(card, player);
				break;
			default:
				UnityEngine.Debug.Log("Invalid Bodhi card ID");
				break;
        }
    }

    private void MottoOfPubbeNivasanussati(CardItem card, PlayerManager player)
    {
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        if (player.stats.hitCount < (int)Mathf.Round(count))
            return;
		
        canBeApplied_01 = false;

        player.stats.scaleAttack += scalar / 100.0f;

        Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t => 
		{
			player.stats.hitCount = 0;
			player.stats.scaleAttack -= scalar / 100.0f;
			canBeApplied_01 = true;
		});
    }

	private void MottoOfDibbaSota(CardItem card, PlayerManager player)
	{
        float count = card.count[card.level];
        float duration = card.duration[card.level];
        float scalar = card.scalar[card.level];

        if (player.stats.hitCount < (int)Mathf.Round(count))
            return;

		canBeApplied_02 = false;

        player.stats.scaleDefense += scalar / 100.0f;

        Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
			player.stats.hitCount = 0;
			player.stats.scaleDefense -= scalar / 100.0f;
			canBeApplied_02 = true;
		});
	}

	private void MottoOfAsavakkhaya(CardItem card, PlayerManager player)
	{
		float count = card.count[card.level];
        float duration = card.duration[card.level];

		canBeApplied_03 = false;

		player.stats.hitCount += (int)Mathf.Round(count);

		Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
			canBeApplied_03 = true;
		});
	}
}
