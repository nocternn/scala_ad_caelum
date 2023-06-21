using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCombatManager : MonoBehaviour
{
    [SerializeField] private StatBarController _barEnemyHealth;
    [SerializeField] private StatBarController _barPlayerHealth;
    [SerializeField] private StatBarController _barPlayerSkillPoints;
    [SerializeField] private StatBarController _barPlayerAttackCharge;
    
    [SerializeField] private SkillButtonController _btnActive;
    [SerializeField] private SkillButtonController _btnUltimate;

    [SerializeField] private Image _crosshair;
    
    public void Initialize()
    {
        _barEnemyHealth        = transform.Find("BarHealthEnemy").GetComponentInChildren<StatBarController>();
        _barPlayerHealth       = transform.Find("BarHealthPlayer").GetComponentInChildren<StatBarController>();
        _barPlayerSkillPoints  = transform.Find("BarSkillPlayer").GetComponentInChildren<StatBarController>();
        _barPlayerAttackCharge = transform.Find("BarChargePlayer").GetComponentInChildren<StatBarController>();

        _btnActive   = transform.Find("ButtonActive").GetComponentInChildren<SkillButtonController>();
        _btnUltimate = transform.Find("ButtonUltimate").GetComponentInChildren<SkillButtonController>();

        _crosshair = transform.Find("Crosshair").GetComponentInChildren<Image>();
        ToggleCrosshair(false);
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

    public void ToggleCrosshair(bool show)
    {
        _crosshair.gameObject.SetActive(show);
    }
}
