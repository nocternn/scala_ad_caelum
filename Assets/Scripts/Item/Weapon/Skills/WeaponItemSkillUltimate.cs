using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItemSkillUltimate : WeaponItemSkill
{
  public override void Initialize(WeaponItem weapon, PlayerManager player = null, EnemyManager enemy = null)
  {
    base.Initialize(weapon, player, enemy);

    currentCooldown = weapon.ultimateCooldown;
  }
  
  public override void UseSkill()
  {
    _player.PlayTargetAnimation(_weapon.ultimateAttack, true);
    base.UseSkill();
  }
  
  #region Skill Overrides

  protected override void Pistol()
  {
    int damageOriginal = _player.weaponSlotManager.GetDamage();
    int damageExtra    = 1088 * 2;
    int damageOverTime = 266;
    int duration = 5;
    float dmgReduction = 0.8f;
    float hpLossOverTime = 0.13f;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (_player.stats.currentHealth < _player.stats.ScaleStat(_player.stats.maxHealth, threshold))
      return;

    // Set damage
    _player.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Shoot in front
    _player.attacker.ShootBullet();

    // Revert damage
    _player.weaponSlotManager.SetDamage(damageOriginal);

    // Get damage reduction
    _player.stats.scaleDefense += dmgReduction;

    // Enemmy takes damage per second for duration seconds
    StartCoroutine(ApplyEffect(damageOverTime, duration));

    // Lose HP over time after duration; remove dmg reduction after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      StartCoroutine(ApplyEffect(hpLossOverTime, duration));
      _player.stats.scaleDefense -= dmgReduction;
		});
  }

  protected override void Greatsword()
  {
    float dmgBoost = 1.0f;
    float dmgReduction = 0.5f;
    int duration = 10;

    // Gain stats
    _player.stats.scaleAttack += dmgBoost;
    _player.stats.scaleDefense += dmgReduction;

    // Reset stats after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.stats.scaleAttack -= dmgBoost;
      _player.stats.scaleDefense -= dmgReduction;
		});
  }

  protected override void Gauntlet()
  {
    int damageOriginal = _player.weaponSlotManager.GetDamage();
    int damageExtra    = _player.stats.ScaleStat(damageOriginal, 1.5f);
    int duration = 10;
    int hpRestore = 20;
    int spRestore = 3;

    // Set damage
    _player.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Restore HP and SP
    StartCoroutine(ApplyEffect(hpRestore, spRestore, duration));

    // Reset damage
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Katana()
  {
    int damageOriginal = _player.weaponSlotManager.GetDamage();
    int damage = _player.stats.ScaleStat(damageOriginal, 150f);
    int duration = 2;

    // Set damage
    _player.weaponSlotManager.SetDamage(damage);

    // Reset damage
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Scythe()
  {
    int hpRestore = 300;
    float dmgBonus = 0.25f;

    _player.stats.currentHealth = Mathf.Min(_player.stats.currentHealth + hpRestore, _player.stats.maxHealth);
    _player.stats.scaleAttack += dmgBonus;
  }

  #endregion

  #region Helpers

  private IEnumerator ApplyEffect(float scalar, int duration)
  {
    int damage = _player.stats.ScaleStat(_player.stats.maxHealth, scalar);
    for (int i = 1; i <= duration; i++)
    {
        if (_player.stats.currentHealth - damage > 0)
          _player.stats.currentHealth -= damage;
        yield return new WaitForSeconds(1.0f);
    }
  }

  private IEnumerator ApplyEffect(int hp, int sp, int duration)
  {
    for (int i = 1; i <= duration; i++)
    {
      _player.stats.currentHealth = Mathf.Min(_player.stats.currentHealth + hp, _player.stats.maxHealth);
      _player.stats.currentSkillPoints = Mathf.Min(_player.stats.currentSkillPoints + sp, _player.stats.maxSkillPoints);
      yield return new WaitForSeconds(1.0f);
    }
  }

  #endregion
}
