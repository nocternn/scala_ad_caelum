using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager singleton;

    private StatBarController _barEnemyHealth;
    private StatBarController _barPlayerHealth;
    private StatBarController _barPlayerSkillPoints;
    private StatBarController _barPlayerAttackCharge;
    
    private SkillButtonController _btnActive;
    private SkillButtonController _btnUltimate;
    
    public void Initialize()
    {
        singleton = this;
        
        _barEnemyHealth        = transform.Find("BarHealthEnemy").GetComponent<StatBarController>();
        _barPlayerHealth       = transform.Find("BarHealthPlayer").GetComponent<StatBarController>();
        _barPlayerSkillPoints  = transform.Find("BarSkillPlayer").GetComponent<StatBarController>();
        _barPlayerAttackCharge = transform.Find("BarChargePlayer").GetComponent<StatBarController>();

        _btnActive   = transform.Find("ButtonActive").GetComponent<SkillButtonController>();
        _btnUltimate = transform.Find("ButtonUltimate").GetComponent<SkillButtonController>();
    }
    
    public void UpdateUI(WeaponItem weapon)
    {
        UpdateSkillButtonsUI(weapon);
    }
    public void UpdateUI(string tag, int currentValue, int maxValue)
    {
        if (tag.Equals("player_health"))
        {
            UpdateHealthUI("player", currentValue, maxValue);
        }
        else if (tag.Equals("enemy_health"))
        {
            UpdateHealthUI("enemy", currentValue, maxValue);
        }
        else if (tag.Equals("skill"))
        {
            UpdatePlayerSkillPointsUI(currentValue, maxValue);
        }
        else if (tag.Equals("charge"))
        {
            UpdatePlayerAttackCharge(currentValue, maxValue);
        }
    }

    private void UpdateHealthUI(string tag, int currentHealth, int maxHealth)
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
    
    private void UpdatePlayerSkillPointsUI(int currentPoints, int maxPoints)
    {
        _barPlayerSkillPoints.UpdateUI(currentPoints, maxPoints);
    }
    
    private void UpdatePlayerAttackCharge(int currentCharge, int maxCharge)
    {
        _barPlayerAttackCharge.SetMaxValue(maxCharge);
        _barPlayerAttackCharge.SetValue(currentCharge);
    }

    private void UpdateSkillButtonsUI(WeaponItem weaponItem)
    {
        _btnActive.UpdateUI(weaponItem);
        _btnUltimate.UpdateUI(weaponItem);
    }
    
}
