using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private StageManager _stage;
    
    private HUDCombatManager _combat;
    private HUDBuffManager _buff;

    public void Initialize()
    {
        Transform combat = transform.GetChild(0);
        Transform buff = transform.GetChild(1);
        
        combat.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
        
        if (_stage.type == Enums.StageType.Combat)
        {
            combat.gameObject.SetActive(true);
            
            _combat = combat.GetComponent<HUDCombatManager>();
            _combat.Initialize();
        }
        else if (_stage.type == Enums.StageType.Buff)
        {
            buff.gameObject.SetActive(true);

            _buff = buff.GetComponent<HUDBuffManager>();
            _buff.Initialize();
        }
    }
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }
    
    void Update()
    {
        if (_stage.type == Enums.StageType.Combat)
        {
            _combat.UpdateHealthUI("player", _stage.player.stats.currentHealth, _stage.player.stats.maxHealth);
            _combat.UpdateHealthUI("enemy", _stage.enemy.stats.currentHealth, _stage.enemy.stats.maxHealth);
            _combat.UpdatePlayerSkillPointsUI((int)Mathf.Round(_stage.player.stats.currentSkillPoints), _stage.player.stats.maxSkillPoints);
            _combat.UpdatePlayerAttackCharge(_stage.player.stats.currentCharge, PlayerStats.MaxCharge);
        }
    }
    
    public void UpdateUI(WeaponItem weapon)
    {
        _combat.UpdateSkillButtonsUI(weapon);
    }

    public void SwitchToCombat()
    {
        if (_buff.selectedCard != null)
        {
            _stage.SwitchToCombat(_buff.selectedCard);
        }
    }
}
