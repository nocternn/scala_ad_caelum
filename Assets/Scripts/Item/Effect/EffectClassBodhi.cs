using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class EffectClassBodhi : EffectClass
{
    public override void Apply(EffectItem effect)
    {
        switch (effect.id)
        {
            case 1:
	            if (canBeApplied_01) MottoOfPubbeNivasanussati(effect);
                break;
			case 2:
				if (canBeApplied_02) MottoOfDibbaSota(effect);
				break;
			case 3:
				if (canBeApplied_03) MottoOfAsavakkhaya(effect);
				break;
			default:
				UnityEngine.Debug.Log("Invalid Bodhi effect ID");
				break;
        }
    }

    private void MottoOfPubbeNivasanussati(EffectItem effect)
    {
        float count = effect.count[effect.level];
        float duration = effect.duration[effect.level];
        float scalar = effect.scalar[effect.level];

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

	private void MottoOfDibbaSota(EffectItem effect)
	{
        float count = effect.count[effect.level];
        float duration = effect.duration[effect.level];
        float scalar = effect.scalar[effect.level];

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

	private void MottoOfAsavakkhaya(EffectItem effect)
	{
		float count = effect.count[effect.level];
        float duration = effect.duration[effect.level];

		canBeApplied_03 = false;

		PlayerManager.Instance.stats.hitCount += (int)Mathf.Round(count);

		Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
			canBeApplied_03 = true;
		});
	}
}
