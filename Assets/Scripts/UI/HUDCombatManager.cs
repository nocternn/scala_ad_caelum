using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCombatManager : MonoBehaviour
{
    private StatBarController _barEnemyHealth;
    private StatBarController _barPlayerHealth;
    private StatBarController _barPlayerSkillPoints;
    private StatBarController _barPlayerAttackCharge;
    
    private SkillButtonController _btnActive;
    private SkillButtonController _btnUltimate;
    
    public void Initialize()
    {
        _barEnemyHealth        = transform.Find("BarHealthEnemy").GetComponentInChildren<StatBarController>();
        _barPlayerHealth       = transform.Find("BarHealthPlayer").GetComponentInChildren<StatBarController>();
        _barPlayerSkillPoints  = transform.Find("BarSkillPlayer").GetComponentInChildren<StatBarController>();
        _barPlayerAttackCharge = transform.Find("BarChargePlayer").GetComponentInChildren<StatBarController>();

        _btnActive   = transform.Find("ButtonActive").GetComponentInChildren<SkillButtonController>();
        _btnUltimate = transform.Find("ButtonUltimate").GetComponentInChildren<SkillButtonController>();
    }

    public void UpdateHealthUI(string tag, int currentHealth, int maxHealth)
    {
        if (tag.Equals("player"))
        {
            _barPlayerHealth.UpdateUI(currentHealth, maxHealth);
        }
        else
        {
            _barEnemyHealth.UpdateUI(currentHealth, maxHealth);
        }
    }
    
    public void UpdatePlayerSkillPointsUI(int currentPoints, int maxPoints)
    {
        _barPlayerSkillPoints.UpdateUI(currentPoints, maxPoints);
    }
    
    public void UpdatePlayerAttackCharge(int currentCharge, int maxCharge)
    {
        _barPlayerAttackCharge.SetMaxValue(maxCharge);
        _barPlayerAttackCharge.SetValue(currentCharge);
    }

    public void UpdateSkillButtonsUI(WeaponItem weaponItem)
    {
        _btnActive.UpdateUI(weaponItem);
        _btnUltimate.UpdateUI(weaponItem);
    }
}
