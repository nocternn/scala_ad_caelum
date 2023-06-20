using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    #region Attributes
    
    private StageManager _stage;
    
    [Header("HUDs")]
    public Transform combat;
    public Transform buff;
    public Transform shop;
    public Transform stage;
    
    [Header("HUD Managers")]
    public HUDCombatManager hudCombat;
    public HUDBuffManager hudBuff;
    public HUDShopManager hudShop;
    public HUDStageManager hudStage;

    private bool _initialized;

    #endregion

    void Start()
    {
        _initialized = false;
    }
    
    void Update()
    {
        if (!_initialized)
            return;
        
        if (_stage.stageType == Enums.StageType.Combat)
        {
            hudCombat.UpdateHealthUI("player", _stage.player.stats.currentHealth, _stage.player.stats.maxHealth);
            hudCombat.UpdateHealthUI("enemy", _stage.enemy.stats.currentHealth, _stage.enemy.stats.maxHealth);
            hudCombat.UpdatePlayerSkillPointsUI((int)Mathf.Round(_stage.player.stats.currentSkillPoints), _stage.player.stats.maxSkillPoints);
            hudCombat.UpdatePlayerAttackCharge(_stage.player.stats.currentCharge, PlayerStats.MaxCharge);
        }
    }
    

    public void Initialize()
    {
        combat = transform.GetChild(0);
        buff = transform.GetChild(1);
        shop = transform.GetChild(2);
        stage = transform.GetChild(3);
        
        combat.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        
        hudStage = stage.GetComponent<HUDStageManager>();
        hudStage.SetManager(_stage);
        hudStage.Initialize();
        hudStage.coin.SetCoins(_stage.coinsAvailable);
        
        if (_stage.stageType == Enums.StageType.Combat)
        {
            combat.gameObject.SetActive(true);
            
            hudCombat = combat.GetComponent<HUDCombatManager>();
            hudCombat.Initialize();

            hudStage.timer.gameObject.SetActive(true);
        }
        else if (_stage.stageType == Enums.StageType.Buff)
        {
            buff.gameObject.SetActive(true);

            hudBuff = buff.GetComponent<HUDBuffManager>();
            hudBuff.Initialize();
        }
        else if (_stage.stageType == Enums.StageType.Shop)
        {
            shop.gameObject.SetActive(true);
            
            hudShop = shop.GetComponent<HUDShopManager>();
            hudShop.SetManager(_stage);
            hudShop.Initialize();
        }

        _initialized = true;
    }
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }
    
    public void UpdateUI(WeaponItem weapon)
    {
        hudCombat.UpdateSkillButtonsUI(weapon);
    }

    public void SwitchToCombat()
    {
        if (hudBuff.selectedCard != null)
        {
            _stage.SwitchToCombat(hudBuff.selectedCard);
        }
    }
}
