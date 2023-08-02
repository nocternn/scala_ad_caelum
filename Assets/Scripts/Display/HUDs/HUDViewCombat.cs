using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDViewCombat : MonoBehaviour
{
    [SerializeField] private StatBarView _barEnemyHealth;
    [SerializeField] private StatBarView _barPlayerHealth;
    [SerializeField] private StatBarView _barPlayerSkillPoints;
    [SerializeField] private StatBarView _barPlayerAttackCharge;
    
    [SerializeField] private SkillButtonView _btnActive;
    [SerializeField] private SkillButtonView _btnUltimate;

    [SerializeField] private Image _crosshair;
    
    public void Initialize()
    {
        Cursor.lockState = CursorLockMode.Locked;
            
        _barEnemyHealth        = transform.Find("BarHealthEnemy").GetComponentInChildren<StatBarView>();
        _barPlayerHealth       = transform.Find("BarHealthPlayer").GetComponentInChildren<StatBarView>();
        _barPlayerSkillPoints  = transform.Find("BarSkillPlayer").GetComponentInChildren<StatBarView>();
        _barPlayerAttackCharge = transform.Find("BarChargePlayer").GetComponentInChildren<StatBarView>();

        _btnActive   = transform.Find("ButtonActive").GetComponentInChildren<SkillButtonView>();
        _btnUltimate = transform.Find("ButtonUltimate").GetComponentInChildren<SkillButtonView>();

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
