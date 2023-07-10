using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItemSkillActive : WeaponItemSkill
{
  #region Skill Overrides

  protected override void Pistol()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int damageExtra    = 1088;
    int duration = 5;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (PlayerManager.Instance.stats.currentHealth < PlayerManager.Instance.stats.ScaleStat(PlayerManager.Instance.stats.maxHealth, threshold))
      return;

    // Lost half of current health
    PlayerManager.Instance.stats.currentHealth /= 2;

    // Set damage
    PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Shoot 1 bullet per second for duration seconds
    StartCoroutine(ShootBullets(duration));

    // Set damage to original damage after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Greatsword()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int damageExtra = PlayerManager.Instance.stats.ScaleStat(damageOriginal, 0.5f);
    int shield = PlayerManager.Instance.stats.ScaleStat(PlayerManager.Instance.stats.maxHealth, 0.8f);
    int duration = 10;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (PlayerManager.Instance.stats.currentHealth < PlayerManager.Instance.stats.ScaleStat(PlayerManager.Instance.stats.maxHealth, threshold))
      return;

    // Lost half of current health
    PlayerManager.Instance.stats.currentHealth /= 2;

    int healthOriginal = PlayerManager.Instance.stats.currentHealth;

    // Gain extra damage and shield
    PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal + damageExtra);
    PlayerManager.Instance.stats.currentHealth += shield;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal);
      PlayerManager.Instance.stats.currentHealth = Mathf.Min(PlayerManager.Instance.stats.currentHealth, healthOriginal);
		});  
  }

  protected override void Gauntlet()
  {
    int gap = 3;
    int maxStacks = 10;
    int duration = 10;
    float dmgBoost = 0.2f;

    // Get dmg multiplier
    int multiplier = Mathf.Min(PlayerManager.Instance.stats.hitCount / gap, maxStacks);

    // Gain dmg bonus
    PlayerManager.Instance.stats.scaleAttack += multiplier * dmgBoost;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.stats.scaleAttack -= multiplier * dmgBoost;
		}); 
  }

  protected override void Katana()
  {
    float dmgBoost = 0.5f;
    int duration = 20;

    // Gain dmg bonus
    PlayerManager.Instance.stats.scaleAttack += dmgBoost;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.stats.scaleAttack -= dmgBoost;
		}); 
  }

  protected override void Scythe()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int duration = 10;

    StartCoroutine(ApplyEffect(damageOriginal, duration));
  }

  #endregion

  #region Helpers

  private IEnumerator ShootBullets(int duration)
  {
    for (int i = 1; i <= duration; i++)
    {
      PlayerManager.Instance.HandleAttack(Enums.ActionType.Shoot);
      yield return new WaitForSeconds(0.5f);
    }
  }

  #endregion
}
