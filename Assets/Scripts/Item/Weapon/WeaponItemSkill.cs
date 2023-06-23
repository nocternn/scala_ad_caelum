using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemSkill : MonoBehaviour
{
    protected WeaponItem _weapon;
    
    [Header("Properties")]
    public bool onCooldown;
    public int currentCooldown;

    public virtual void Initialize(WeaponItem weapon)
    {
        _weapon = weapon;

        onCooldown = false;
    }
    
    public void Cooldown()
    {
        onCooldown = true;
        StartCoroutine(CooldownTimer(currentCooldown));
    }

    public virtual void UseSkill()
    {
      switch (_weapon.id)
      {
          case 1:
              Pistol();
              break;
          case 2:
              Greatsword();
              break;
          case 3:
              Gauntlet();
              break;
          case 4:
              Katana();
              break;
          case 5:
              Scythe();
              break;
          default:
              Debug.Log("Invalid weapon ID");
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
            EnemyManager.Instance.stats.currentHealth -= damage;
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    private IEnumerator CooldownTimer(int initialCooldown)
    {
        while (currentCooldown >= 0)
        {
            HUDManager.Instance.hudCombat.UpdateSkillButtonsUI(_weapon);
            currentCooldown--;
            yield return new WaitForSeconds(1.0f);
        }
        
        onCooldown = false;
        currentCooldown = initialCooldown;
        HUDManager.Instance.hudCombat.UpdateSkillButtonsUI(_weapon);

        yield break;
    }

    #endregion
}
