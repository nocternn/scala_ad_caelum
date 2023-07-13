using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    #region Attributes
    
    public static HUDManager Instance { get; private set; }

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
        Instance = this;
        
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
        
        if (StageManager.Instance.stageType == Enums.StageType.Combat)
        {
            hudCombat.UpdateHealthUI("player",
                    PlayerManager.Instance.stats.currentHealth,
                    PlayerManager.Instance.stats.maxHealth
                );
            hudCombat.UpdateHealthUI("enemy",
                    EnemyManager.Instance.stats.currentHealth,
                    EnemyManager.Instance.stats.maxHealth
                );
            hudCombat.UpdatePlayerSkillPointsUI(
                    (int)Mathf.Round(PlayerManager.Instance.stats.currentSkillPoints),
                    PlayerManager.Instance.stats.maxSkillPoints
                );
            hudCombat.UpdatePlayerAttackCharge(PlayerManager.Instance.stats.currentCharge, PlayerStats.MaxCharge);
        }
    }

    public void Initialize()
    {
        combat.gameObject.SetActive(false);
        buff.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        
        hudStage.Initialize();
        hudStage.coin.SetCoins(StageManager.Instance.coinsAvailable);
        
        if (StageManager.Instance.stageType == Enums.StageType.Combat)
        {
            combat.gameObject.SetActive(true);
            
            hudCombat = combat.GetComponent<HUDCombatManager>();
            hudCombat.Initialize();
            
            hudCombat.UpdateSkillButtonsUI(StatisticsManager.Instance.playerWeapon);

            hudStage.timer.gameObject.SetActive(true);
        }
        else if (StageManager.Instance.stageType == Enums.StageType.Dialogue)
        {
            hudStage.dialogue.SetDialogues(StageManager.Instance.dialogue.GetDialogues(StageManager.Instance.id));
            hudStage.dialogue.StartDialogue();
        }
        else if (StageManager.Instance.stageType == Enums.StageType.Buff)
        {
            buff.gameObject.SetActive(true);

            hudBuff = buff.GetComponent<HUDBuffManager>();
            hudBuff.Initialize();
        }
        else if (StageManager.Instance.stageType == Enums.StageType.Shop)
        {
            shop.gameObject.SetActive(true);

            StageManager.Instance.shop.isOpen = true;
            StageManager.Instance.shop.isOpenable = false;
            
            hudShop = shop.GetComponent<HUDShopManager>();
            hudShop.Initialize();
        }

        _initialized = true;
    }

    public void Action(Enums.HUDAction action)
    {
        switch (action)
        {
            case Enums.HUDAction.Back:
                StageManager.Instance.Back();
                break;
            case Enums.HUDAction.Interact:
                StageManager.Instance.Interact();
                break;
            case Enums.HUDAction.Quit:
                StageManager.Instance.Quit();
                break;
            case Enums.HUDAction.SwitchBuff:
                StageManager.Instance.SwitchToBuff();
                break;
            case Enums.HUDAction.SwitchCombat:
                if (hudBuff.selectedCard != null)
                {
                    StageManager.Instance.SwitchToCombat(hudBuff.selectedCard);
                }
                break;
            default:
                Debug.Log("Invalid HUD action");
                break;
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return PlayerManager.Instance.transform.position;
    }
}
