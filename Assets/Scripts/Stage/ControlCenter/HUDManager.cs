using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    #region Attributes
    
    public static HUDManager Instance { get; private set; }
    
    [Header("HUD Managers")]
    public HUDViewCombat hudCombat;
    public HUDViewEffects hudEffects;
    public HUDViewShop hudShop;
    public HUDViewStage hudStage;

    [SerializeField] private bool _initialized;

    #endregion

    void Awake()
    {
        Instance = this;
        
        _initialized = false;
        
        hudCombat = GameObject.FindObjectsOfType<HUDViewCombat>(true)[0];
        hudStage = GameObject.FindObjectsOfType<HUDViewStage>(true)[0];
    }
    
    void Update()
    {
        if (!_initialized)
            return;
        
        if (StageManager.Instance.hudType == Enums.HUDType.Combat)
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

    public void Initialize(bool firstInit = false)
    {
        Cursor.lockState = CursorLockMode.Confined;
        hudCombat.gameObject.SetActive(false);
        
        if (!StageManager.Instance.isLocalBattle && firstInit)
        {
            hudEffects = GameObject.FindObjectsOfType<HUDViewEffects>(true)[0];
            hudShop = GameObject.FindObjectsOfType<HUDViewShop>(true)[0];
            
            // Reorder so that:
            // - combat is the first child in hierarchy
            // - stage is the last child in hierarchy
            hudEffects.transform.SetSiblingIndex(1);
            hudShop.transform.SetSiblingIndex(1);
        }

        if (!StageManager.Instance.isLocalBattle)
        {
            hudEffects.gameObject.SetActive(false);
            hudShop.gameObject.SetActive(false);
        }
        
        hudStage.Initialize();
        if (!StageManager.Instance.isLocalBattle)
            hudStage.currency.SetCoins(StageManager.Instance.coinsAvailable);
        
        if (StageManager.Instance.hudType == Enums.HUDType.Combat)
        {
            Cursor.lockState = CursorLockMode.Locked;
            hudCombat.gameObject.SetActive(true);
            hudCombat.Initialize();
            hudCombat.UpdateSkillButtonsUI(StatisticsManager.Instance.playerWeapon);

            hudStage.timer.gameObject.SetActive(true);
        }
        else if (StageManager.Instance.hudType == Enums.HUDType.Dialogue)
        {
            hudStage.dialogue.SetDialogues(StageManager.Instance.dialogue.GetDialogues(StageManager.Instance.id));
            hudStage.dialogue.StartDialogue();
        }
        else if (StageManager.Instance.hudType == Enums.HUDType.Effects)
        {
            hudEffects.gameObject.SetActive(true);
            hudEffects.Initialize();
        }
        else if (StageManager.Instance.hudType == Enums.HUDType.Shop)
        {
            hudShop.gameObject.SetActive(true);
            hudShop.Initialize();
        }

        _initialized = true;
    }

    public void Action(Enums.HUDActionType action)
    {
        switch (action)
        {
            case Enums.HUDActionType.Back:
                StageManager.Instance.Back();
                break;
            case Enums.HUDActionType.Interact:
                StageManager.Instance.Interact();
                break;
            case Enums.HUDActionType.Quit:
                StageManager.Instance.Quit();
                break;
            case Enums.HUDActionType.SwitchEffects:
                StageManager.Instance.SwitchToEffects();
                break;
            case Enums.HUDActionType.SwitchCombat:
                if (hudEffects.selectedCard != null)
                {
                    StageManager.Instance.SwitchToCombat(hudEffects.selectedCard);
                }
                break;
            default:
                Debug.Log("Invalid HUD action");
                break;
        }
    }

    public void HandleStageWin(int reward) {
        // Stop stage timer
        hudStage.ToggleTimer(false);
        
        // Show combat report
        hudStage.InitializeCombatReport(reward);
        hudStage.ShowCombatReport(true, !StageManager.Instance.isLocalBattle);
        
        // Register coin rewards
        if (!StageManager.Instance.isLocalBattle)
        {
            hudStage.AddCoins(true, reward);
            StageManager.Instance.coinsAvailable = hudStage.currency.GetCoins();
        }
    }
}
