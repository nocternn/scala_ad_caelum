using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItemSkillUltimate : WeaponItemSkill
{
  public override void Initialize(WeaponItem weapon)
  {
    base.Initialize(weapon);

    currentCooldown = weapon.ultimateCooldown;
  }
  
  public override void UseSkill()
  {
    PlayerManager.Instance.PlayTargetAnimation(_weapon.ultimateAttack, true);
    base.UseSkill();
  }
  
  #region Skill Overrides

  protected override void Pistol()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int damageExtra    = 1088 * 2;
    int damageOverTime = 266;
    int duration = 5;
    float dmgReduction = 0.8f;
    float hpLossOverTime = 0.13f;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (PlayerManager.Instance.stats.currentHealth < PlayerManager.Instance.stats.ScaleStat(PlayerManager.Instance.stats.maxHealth, threshold))
      return;

    // Set damage
    PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Shoot in front
    PlayerManager.Instance.attacker.ShootBullet();

    // Revert damage
    PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal);

    // Get damage reduction
    PlayerManager.Instance.stats.scaleDefense += dmgReduction;

    // Enemmy takes damage per second for duration seconds
    StartCoroutine(ApplyEffect(damageOverTime, duration));

    // Lose HP over time after duration; remove dmg reduction after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      StartCoroutine(ApplyEffect(hpLossOverTime, duration));
      PlayerManager.Instance.stats.scaleDefense -= dmgReduction;
		});
  }

  protected override void Greatsword()
  {
    float dmgBoost = 1.0f;
    float dmgReduction = 0.5f;
    int duration = 10;

    // Gain stats
    PlayerManager.Instance.stats.scaleAttack += dmgBoost;
    PlayerManager.Instance.stats.scaleDefense += dmgReduction;

    // Reset stats after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.stats.scaleAttack -= dmgBoost;
      PlayerManager.Instance.stats.scaleDefense -= dmgReduction;
		});
  }

  protected override void Gauntlet()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int damageExtra    = PlayerManager.Instance.stats.ScaleStat(damageOriginal, 1.5f);
    int duration = 10;
    int hpRestore = 20;
    int spRestore = 3;

    // Set damage
    PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Restore HP and SP
    StartCoroutine(ApplyEffect(hpRestore, spRestore, duration));

    // Reset damage
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Katana()
  {
    int damageOriginal = PlayerManager.Instance.weaponSlotManager.GetDamage();
    int damage = PlayerManager.Instance.stats.ScaleStat(damageOriginal, 150f);
    int duration = 2;

    // Set damage
    PlayerManager.Instance.weaponSlotManager.SetDamage(damage);

    // Reset damage
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      PlayerManager.Instance.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Scythe()
  {
    int hpRestore = 300;
    float dmgBonus = 0.25f;

    PlayerManager.Instance.stats.currentHealth = Mathf.Min(
        PlayerManager.Instance.stats.currentHealth + hpRestore,
        PlayerManager.Instance.stats.maxHealth
      );
    PlayerManager.Instance.stats.scaleAttack += dmgBonus;
  }

  #endregion

  #region Helpers

  private IEnumerator ApplyEffect(float scalar, int duration)
  {
    int damage = PlayerManager.Instance.stats.ScaleStat(PlayerManager.Instance.stats.maxHealth, scalar);
    for (int i = 1; i <= duration; i++)
    {
        if (PlayerManager.Instance.stats.currentHealth - damage > 0)
          PlayerManager.Instance.stats.currentHealth -= damage;
        yield return new WaitForSeconds(1.0f);
    }
  }

  private IEnumerator ApplyEffect(int hp, int sp, int duration)
  {
    for (int i = 1; i <= duration; i++)
    {
      PlayerManager.Instance.stats.currentHealth = Mathf.Min(PlayerManager.Instance.stats.currentHealth + hp, PlayerManager.Instance.stats.maxHealth);
      PlayerManager.Instance.stats.currentSkillPoints = Mathf.Min(PlayerManager.Instance.stats.currentSkillPoints + sp, PlayerManager.Instance.stats.maxSkillPoints);
      yield return new WaitForSeconds(1.0f);
    }
  }

  #endregion
}
