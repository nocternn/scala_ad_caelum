using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItemSkillActive : WeaponItemSkill
{
  #region Skill Overrides

  protected override void Pistol()
  {
    int damageOriginal = _player.weaponSlotManager.GetDamage();
    int damageExtra    = 1088;
    int duration = 5;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (_player.stats.currentHealth < _player.stats.ScaleStat(_player.stats.maxHealth, threshold))
      return;

    // Lost half of current health
    _player.stats.currentHealth /= 2;

    // Set damage
    _player.weaponSlotManager.SetDamage(damageOriginal + damageExtra);

    // Shoot 1 bullet per second for duration seconds
    StartCoroutine(ShootBullets(duration));

    // Set damage to original damage after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.weaponSlotManager.SetDamage(damageOriginal);
		});
  }

  protected override void Greatsword()
  {
    int damageOriginal = _player.weaponSlotManager.GetDamage();
    int damageExtra = _player.stats.ScaleStat(damageOriginal, 0.5f);
    int shield = _player.stats.ScaleStat(_player.stats.maxHealth, 0.8f);
    int duration = 10;
    float threshold = 0.33f;

    // If current health is below 33% then don't attack
    if (_player.stats.currentHealth < _player.stats.ScaleStat(_player.stats.maxHealth, threshold))
      return;

    // Lost half of current health
    _player.stats.currentHealth /= 2;

    int healthOriginal = _player.stats.currentHealth;

    // Gain extra damage and shield
    _player.weaponSlotManager.SetDamage(damageOriginal + damageExtra);
    _player.stats.currentHealth += shield;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.weaponSlotManager.SetDamage(damageOriginal);
      _player.stats.currentHealth = Mathf.Min(_player.stats.currentHealth, healthOriginal);
		});  
  }

  protected override void Gauntlet()
  {
    int gap = 3;
    int maxStacks = 10;
    int duration = 10;
    float dmgBoost = 0.2f;

    // Get dmg multiplier
    int multiplier = Mathf.Min(_player.stats.hitCount / gap, maxStacks);

    // Gain dmg bonus
    _player.stats.scaleAttack += multiplier * dmgBoost;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.stats.scaleAttack -= multiplier * dmgBoost;
		}); 
  }

  protected override void Katana()
  {
    float dmgBoost = 0.5f;
    int duration = 20;

    // Gain dmg bonus
    _player.stats.scaleAttack += dmgBoost;

    // Remove bonuses after duration seconds
    Task.Delay((int)Mathf.Round(1000 * duration)).ContinueWith(t =>
		{
      _player.stats.scaleAttack -= dmgBoost;
		}); 
  }

  protected override void Scythe()
  {
  Debug.Log("Use Scythe Active");
  }

  #endregion

  #region Helpers

  private IEnumerator ShootBullets(int duration)
  {
    for (int i = 1; i <= duration; i++)
    {
      _player.PlayTargetAnimation(_weapon.activeAttack, true);
      _player.attacker.ShootBullet();
      yield return new WaitForSeconds(1.0f);
    }
  }

  #endregion
}
