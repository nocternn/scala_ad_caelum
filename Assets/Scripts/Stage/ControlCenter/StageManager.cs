		using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Attributes
    
    public static StageManager Instance { get; private set; }
    
    [Header("Stage elements")]
    public CoinManager coin;
    public DoorManager door;
    public DialogueManager dialogue;
    public ShopManager shop;

    [Header("Effects")]
    public List<EffectItem> selectedEffects;
    public GameObject effectsHolder;

    [Header("Properties")]
    public int id;
    public int coinsAvailable;
    public bool isLocalBattle;
    public Enums.HUDType hudType;
    public Enums.HUDType previousHudType;
    [SerializeField] private int _clearReward;
    [SerializeField] private bool _initialized;

    #endregion

    #region Lifecycle
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        door = GameObject.FindObjectsOfType<DoorManager>(true)[0];
        shop = GameObject.FindObjectsOfType<ShopManager>(true)[0];
        coin     = GetComponent<CoinManager>();
        dialogue = GetComponent<DialogueManager>();

        effectsHolder = transform.GetChild(0).gameObject;
    }
    
    private void Update()
    {
        if (!_initialized)
            return;
        
        if (hudType != Enums.HUDType.Combat)
            return;
    
        foreach (EffectItem effect in selectedEffects)
            effect.Apply();
    }

    #endregion

    #region Initiate
    
    public void Initialize(bool firstInit = false)
    {
        StatisticsManager.Instance.playerWeapon.InitializeSkills();
        
        door.Initialize();

        if (!isLocalBattle)
        {
            coin.Initialize();
            dialogue.Initialize();
            shop.Initialize();
            
            HUDManager.Instance.Initialize(firstInit);
        }
        else
        {
            SwitchToCombat();
        }
        
        _initialized = true;
    }
    
    public void Reset()
    {
        hudType = Enums.HUDType.Dialogue;
        previousHudType = Enums.HUDType.Dialogue;

        _clearReward = 100;
        _initialized = false;

        id = 1;
        coinsAvailable = 0;
        
        selectedEffects.Clear();

        foreach (var comp in effectsHolder.GetComponents<Component>())
        {
            if (!(comp is Transform))
            {
                Destroy(comp);
            }
        }
    }

    #endregion

    #region Toggles

    private void TogglePlayer(bool show)
    {
        if (show)
        {
            CameraManager.Instance.SetCamera(Enums.CameraType.Standard);

            PlayerManager.Instance.ToggleActive(true);
            PlayerManager.Instance.SetWeapon(StatisticsManager.Instance.playerWeapon);
            PlayerManager.Instance.stats.CalculateCritChance(id);
        }
        else
        {
            PlayerManager.Instance.ToggleActive(false);
            
            CameraManager.Instance.SetCamera(Enums.CameraType.Free);
        }
    }

    private void ToggleEnemy(bool show)
    {
        if (show)
        {
            EnemyManager.Instance.ToggleActive(true);
            EnemyManager.Instance.SetEnemyType(Dictionaries.EnemyType[(Enums.EnemyType)id]);
            if (!isLocalBattle)
            {
                EnemyManager.Instance.SetWeapon(EnemyManager.Instance.weaponSlotManager.leftHandWeapon);
                EnemyManager.Instance.SetWeapon(EnemyManager.Instance.weaponSlotManager.rightHandWeapon);
            }
            else
            {
                EnemyManager.Instance.SetWeapon(StatisticsManager.Instance.opponentWeapon);
            }
            EnemyManager.Instance.stats.CalculateCritChance(id);
        }
        else
        {
            EnemyManager.Instance.ToggleActive(false);
        }
    }

    #endregion

    #region SwitchStages

    public void SwitchToEffects()
    {
        previousHudType = hudType;
        hudType = Enums.HUDType.Effects;

        if (previousHudType == Enums.HUDType.Dialogue)
        {
            HUDManager.Instance.hudStage.ShowDialogue(false);
        }

        HUDManager.Instance.Initialize();
    }
    
    public void SwitchToCombat(EffectItem selectedEffect = null)
    {
        if (!isLocalBattle)
        {
            selectedEffect.EnableEffect();
            selectedEffects.Add(selectedEffect);
        }

        TogglePlayer(true);
        ToggleEnemy(true);
        
        previousHudType = hudType;
        hudType = Enums.HUDType.Combat;
        
        HUDManager.Instance.Initialize();
    }

    public void SwitchToShop()
    {
        previousHudType = hudType;
        hudType = Enums.HUDType.Shop;

        TogglePlayer(false);
        shop.Open();

        HUDManager.Instance.hudStage.ShowCombatReport(false);
        HUDManager.Instance.Initialize();
    }

    #endregion

    #region SwitchScenes
    
    public void SwitchToNextStage()
    {
        id++;
        _initialized = false;
        door.Open();
        
        bool quit = false;
        if (!isLocalBattle)
        {
            // Register stage progress
            StatisticsManager.Instance.playerStats.progress.stage = id;
            // Register combat stats
            StatisticsManager.Instance.CopyCombatStats(PlayerManager.Instance);
            // Register number of stages cleared stat
            StatisticsManager.Instance.playerStats.meta.numberOfStagesCleared++;
            
            if (id <= StatisticsProgress.MaxStages)
            {
                hudType = Enums.HUDType.Dialogue;
                previousHudType = Enums.HUDType.Dialogue;
            }
            else
            {
                quit = true;
            
                // Register iteration progress
                // If current iteration is last and we passed the last stage then loop back to the beginning
                if (StatisticsManager.Instance.playerStats.progress.iteration == StatisticsProgress.MaxIterations)
                    StatisticsManager.Instance.ResetStatsPlayer(Enums.StatsType.Progress);
            
                // Register number of runs stat
                StatisticsManager.Instance.playerStats.meta.numberOfRuns++;
            }
        }
        else
        {
			// Register number of local battles won
            StatisticsManager.Instance.playerStats.meta.numberOfLocalBattlesWon++;

			quit = true;
        }
        
        // Register all stats changes
        StatisticsManager.Instance.WriteStatsPlayer();

        // Quite run (if necessary)
        if (quit)
        {
            Quit();
        }
        else
        {
            StartCoroutine(SceneLoader.Instance.LoadScene(0));
        }
    }

    public void EndStageWin()
    {
        PlayerManager.Instance.LockOff();
        HUDManager.Instance.HandleStageWin(_clearReward);
        
        // Spawn coin rewards
        if (!isLocalBattle)
        {
            // Coin reward effect
            coin.Spawn(10);

            // If current stage number is even, show interactive shop
            if (id % 2 == 0)
                shop.ToggleVisibility(true);
        }

        // Show interactive door
        door.ToggleVisibility(true);
    }

    public void EndStageLoss()
    {
        if (!isLocalBattle)
        {
            // Reset stage progress to beginning of current iteration
            StatisticsManager.Instance.playerStats.progress.stage = 1;
            // Register number of deaths cleared stat
            StatisticsManager.Instance.playerStats.meta.numberOfDeaths++;
            // Register all stats changes
            StatisticsManager.Instance.WriteStatsPlayer();
        }
        
        Quit();
    }

    #endregion

    #region Actions

    public void Interact()
    {
        if (door.IsOpenable())
        {
            SwitchToNextStage();
        }
        else if (shop.IsOpenable())
        {
            SwitchToShop();
        }
    }

    public void Back()
    {
        if (hudType == Enums.HUDType.Shop)
        {
            hudType = previousHudType;
            previousHudType = Enums.HUDType.Shop;

            TogglePlayer(true);

            door.ToggleVisibility(true);
            shop.ToggleVisibility(true);
            shop.Close();
            
            HUDManager.Instance.hudStage.ShowCombatReport(true);
            HUDManager.Instance.hudShop.gameObject.SetActive(false);
        }
        else if (hudType == Enums.HUDType.Quit)
        {
            hudType = previousHudType;
            previousHudType = Enums.HUDType.Quit;

            if (hudType == Enums.HUDType.Combat)
            {
                TogglePlayer(true);

                // If the enemy is no longer alive then it means the stage is cleared and show the combat report
                // Else show the enemy and resume timer
                if (!EnemyManager.Instance.Died())
                {
                    HUDManager.Instance.hudStage.ToggleTimer(true);

                    ToggleEnemy(true);
                }
                else
                {
                    door.ToggleVisibility(true);
                    if (id % 2 == 0)
                        shop.ToggleVisibility(true);
                    
                    HUDManager.Instance.hudStage.ShowCombatReport(true);
                }
            }
            else if (hudType == Enums.HUDType.Dialogue)
            {
                HUDManager.Instance.hudStage.ShowDialogue(true);
            }
            
            HUDManager.Instance.hudStage.ShowQuitConfirmation(false);
        }
        else
        {
            previousHudType = hudType;
            hudType = Enums.HUDType.Quit;

            if (previousHudType == Enums.HUDType.Combat)
            {
                HUDManager.Instance.hudStage.ToggleTimer(false);
                
                door.Close();
                shop.Close();
                
                door.ToggleVisibility(false);
                shop.ToggleVisibility(false);
                
                TogglePlayer(false);
                ToggleEnemy(false);
                
                HUDManager.Instance.hudStage.ShowCombatReport(false);
            }
            else if (previousHudType == Enums.HUDType.Dialogue)
            {
                HUDManager.Instance.hudStage.ShowDialogue(false);
            }
            
            HUDManager.Instance.hudStage.ShowQuitConfirmation(true);
        }
    }

    public void Quit()
    {
        if (!isLocalBattle)
            Reset();

        SceneLoader.Instance.sceneType = Enums.SceneType.Menu;
        SceneLoader.Instance.previousSceneType = Enums.SceneType.Game;
        
        StartCoroutine(SceneLoader.Instance.LoadScene(0, true));
    }

    #endregion
}
