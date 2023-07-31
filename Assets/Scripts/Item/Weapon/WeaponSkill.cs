using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : MonoBehaviour
{
    [Header("Properties")]
    public bool onCooldown;
    public int currentCooldown;

    public void Initialize(int cooldown)
    {
        onCooldown = false;
        currentCooldown = cooldown;
    }
    
    public void Cooldown()
    {
        onCooldown = true;
        StartCoroutine(CooldownTimer(currentCooldown));
    }

    public virtual void UseSkill(Enums.WeaponType weaponType)
    {
      switch (weaponType)
      {
          case Enums.WeaponType.Pistol:
              Pistol();
              break;
          case Enums.WeaponType.Greatsword:
              Greatsword();
              break;
          case Enums.WeaponType.Gauntlet:
              Gauntlet();
              break;
          case Enums.WeaponType.Katana:
              Katana();
              break;
          case Enums.WeaponType.Scythe:
              Scythe();
              break;
          default:
              Debug.Log("Invalid weapon");
              break;
      }
    }

    #region SkilsByWeapon

    protected virtual void Pistol() { }
    protected virtual void Greatsword() { }
    protected virtual void Gauntlet() { }
    protected virtual void Katana() { }
    protected virtual void Scythe() { }

    #endregion

    #region Coroutines

    protected IEnumerator ApplyEffect(int damage, int duration)
    {
        for (int i = 1; i <= duration; i++)
        {
            EnemyManager.Instance.stats.TakeDamage(damage);
            if (EnemyManager.Instance.Died())
                yield break;
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    private IEnumerator CooldownTimer(int initialCooldown)
    {
        WeaponItem weapon = PlayerManager.Instance.weaponSlotManager.GetCurrentWeapon();
        
        while (currentCooldown >= 0)
        {
            HUDManager.Instance.hudCombat.UpdateSkillButtonsUI(weapon);
            currentCooldown--;
            yield return new WaitForSeconds(1.0f);
        }
        
        onCooldown = false;
        currentCooldown = initialCooldown;
        HUDManager.Instance.hudCombat.UpdateSkillButtonsUI(weapon);

        yield break;
    }

    #endregion
}
