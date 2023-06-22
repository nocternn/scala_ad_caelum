using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    #region Attributes
    
    [SerializeField] private StageManager _stage;
    
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

    [SerializeField] private bool _initialized;

    #endregion

    void Awake()
    {
        _initialized = false;

        combat = transform.GetChild(0);
        buff = transform.GetChild(1);
        shop = transform.GetChild(2);
        stage = transform.GetChild(3);

        hudStage = stage.GetComponent<HUDStageManager>();
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
        combat.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        
        hudStage.SetManager(this);
        hudStage.Initialize();
        hudStage.coin.SetCoins(_stage.coinsAvailable);
        
        if (_stage.stageType == Enums.StageType.Combat)
        {
            combat.gameObject.SetActive(true);
            
            hudCombat = combat.GetComponent<HUDCombatManager>();
            hudCombat.Initialize();
            
            hudCombat.UpdateSkillButtonsUI(_stage.sceneLoader.weapon);

            hudStage.timer.gameObject.SetActive(true);
        }
        else if (_stage.stageType == Enums.StageType.Dialogue)
        {
            hudStage.dialogue.SetDialogues(_stage.dialogue.GetDialogues(_stage.id));
            hudStage.dialogue.StartDialogue();
        }
        else if (_stage.stageType == Enums.StageType.Buff)
        {
            buff.gameObject.SetActive(true);

            hudBuff = buff.GetComponent<HUDBuffManager>();
            hudBuff.SetManager(this);
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

    public void Action(Enums.HUDAction action)
    {
        switch (action)
        {
            case Enums.HUDAction.Back:
                _stage.Back();
                break;
            case Enums.HUDAction.Interact:
                _stage.Interact();
                break;
            case Enums.HUDAction.Quit:
                _stage.Quit();
                break;
            case Enums.HUDAction.SwitchBuff:
                _stage.SwitchToBuff();
                break;
            case Enums.HUDAction.SwitchCombat:
                if (hudBuff.selectedCard != null)
                {
                    _stage.SwitchToCombat(hudBuff.selectedCard);
                }
                break;
            default:
                Debug.Log("Invalid HUD action");
                break;
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return _stage.player.transform.position;
    }
}
