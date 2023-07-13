using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Attributes
    
    public static StageManager Instance { get; private set; }
    
    [Header("Stage elements")]
    public DialogueManager dialogue;
    public Door door;
    public Shop shop;

    [Header("Buffs")]
    public List<CardItem> selectedBuffs;
    public GameObject buffHolder;

    [Header("Properties")]
    [SerializeField] private bool _initialized;
    [SerializeField] private int _clearReward;
    public int id;
    public int coinsAvailable;
    public Enums.StageType stageType;
    public Enums.StageType previousStageType;

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

        dialogue = GetComponent<DialogueManager>();

        door = GameObject.FindObjectsOfType<Door>(true)[0];
        shop = GameObject.FindObjectsOfType<Shop>(true)[0];
    }
    
    private void Update()
    {
        if (!_initialized)
            return;
        
        if (stageType != Enums.StageType.Combat)
            return;
    
        foreach (CardItem buff in selectedBuffs)
        {
            buff.effect.Apply(buff);
        }
    }

    #endregion

    #region Initiate
    
    public void Initialize()
    {
        StatisticsManager.Instance.playerWeapon.InitializeSkills();
        
        HUDManager.Instance.Initialize();
        door.Initialize();
        shop.Initialize();

        ShowShop(false);
        
        _initialized = true;
    }
    
    public void Reset()
    {
        stageType = Enums.StageType.Dialogue;
        previousStageType = Enums.StageType.Dialogue;

        _clearReward = 100;
        _initialized = false;

        id = 1;
        coinsAvailable = 0;
        
        selectedBuffs.Clear();

        foreach (var comp in buffHolder.GetComponents<Component>())
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
            EnemyManager.Instance.SetEnemyType(Dictionaries.EnemyType[(Enums.EnemyType)(id - 1)]);
            EnemyManager.Instance.ToggleActive(true);
        }
        else
        {
            EnemyManager.Instance.ToggleActive(false);
        }
    }

    #endregion

    #region SwitchStages

    public void SwitchToBuff()
    {
        previousStageType = stageType;
        stageType = Enums.StageType.Buff;

        if (previousStageType == Enums.StageType.Dialogue)
        {
            HUDManager.Instance.hudStage.ShowDialogue(false);
        }

        HUDManager.Instance.Initialize();
    }
    
    public void SwitchToCombat(CardItem selectedBuff)
    {
        selectedBuff.EnableEffect(buffHolder);
        selectedBuffs.Add(selectedBuff);

        TogglePlayer(true);
        ToggleEnemy(true);
        
        previousStageType = stageType;
        stageType = Enums.StageType.Combat;
        
        HUDManager.Instance.Initialize();
    }

    public void SwitchToShop()
    {
        previousStageType = stageType;
        stageType = Enums.StageType.Shop;

        TogglePlayer(false);

        HUDManager.Instance.hudStage.ShowCombatReport(false);
        HUDManager.Instance.Initialize();
    }

    #endregion

    #region SwitchScenes
    
    public void SwitchToNextStage()
    {
        id++;
        
        // Register stage progress
        StatisticsManager.Instance.playerStats.progress.stage = id;
        // Register combat stats
        StatisticsManager.Instance.CopyCombatStats(PlayerManager.Instance);
        
        _initialized = false;

        StageManager.Instance.OpenDoor();

        bool quit = false;
        if (id <= StatisticsProgress.MaxStages)
        {
            stageType = Enums.StageType.Dialogue;
            previousStageType = Enums.StageType.Dialogue;
            
            StartCoroutine(SceneLoader.Instance.LoadScene(0));
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
        
        // Register all stats changes
        StatisticsManager.Instance.WriteStatsPlayer();

        // Quite run (if necessary)
        if (quit)
            Quit();
    }

    public void EndStageWin()
    {
        HUDManager.Instance.hudStage.ToggleTimer(false);
        StageManager.Instance.ShowDoor(true);

        if (id % 2 == 0)
            StageManager.Instance.ShowShop(true);

        // Lock off target if player is locking on
        if (PlayerManager.Instance.IsLockingOnTarget())
            PlayerManager.Instance.inputHandler.HandleLockOnInput();

        // Spawn coin rewards
        HUDManager.Instance.hudStage.coin.Spawn(10);
        HUDManager.Instance.hudStage.AddCoins(true, _clearReward);
        coinsAvailable = HUDManager.Instance.hudStage.coin.GetCoins();

        // Show combat report
        HUDManager.Instance.hudStage.InitializeCombatReport(_clearReward);
        HUDManager.Instance.hudStage.ShowCombatReport(true);
        
        // Register number of stages cleared stat
        StatisticsManager.Instance.playerStats.meta.numberOfStagesCleared++;
        StatisticsManager.Instance.WriteStatsPlayer();
    }

    public void EndStageLoss()
    {
        // Register number of deaths cleared stat
        StatisticsManager.Instance.playerStats.meta.numberOfDeaths++;
        StatisticsManager.Instance.WriteStatsPlayer();
        
        Quit();
    }

    #endregion

    #region Actions

    public void Interact()
    {
        if (door.isOpenable)
        {
            SwitchToNextStage();
        }
        else if (shop.isOpenable)
        {
            SwitchToShop();
        }
    }

    public void Back()
    {
        if (stageType == Enums.StageType.Shop)
        {
            stageType = previousStageType;
            previousStageType = Enums.StageType.Shop;

            TogglePlayer(true);

            ShowDoor(true);
            ShowShop(true);
            
            HUDManager.Instance.hudStage.ShowCombatReport(true);
            HUDManager.Instance.shop.gameObject.SetActive(false);
            StageManager.Instance.shop.isOpen = false;
        }
        else if (stageType == Enums.StageType.Quit)
        {
            stageType = previousStageType;
            previousStageType = Enums.StageType.Quit;

            if (stageType == Enums.StageType.Combat)
            {
                TogglePlayer(true);

                // If the door is active then it means the stage is cleared and show the combat report
                // Else show the enemy and resume timer
                if (!StageManager.Instance.door.gameObject.activeSelf)
                {
                    HUDManager.Instance.hudStage.ToggleTimer(true);

                    ToggleEnemy(true);
                }
                else
                {
                    HUDManager.Instance.hudStage.ShowCombatReport(true);
                }
            }
            else if (stageType == Enums.StageType.Dialogue)
            {
                HUDManager.Instance.hudStage.ShowDialogue(true);
            }
            
            HUDManager.Instance.hudStage.ShowQuitConfirmation(false);
        }
        else
        {
            previousStageType = stageType;
            stageType = Enums.StageType.Quit;

            if (previousStageType == Enums.StageType.Combat)
            {
                HUDManager.Instance.hudStage.ToggleTimer(false);

                TogglePlayer(false);

                if (EnemyManager.Instance != null)
                    ToggleEnemy(false);
            }
            else if (previousStageType == Enums.StageType.Dialogue)
            {
                HUDManager.Instance.hudStage.ShowDialogue(false);
            }
            
            HUDManager.Instance.hudStage.ShowCombatReport(false);
            HUDManager.Instance.hudStage.ShowQuitConfirmation(true);
        }
    }

    public void Quit()
    {
        Reset();
        
        SceneLoader.Instance.sceneType = Enums.SceneType.Menu;
        SceneLoader.Instance.previousSceneType = Enums.SceneType.Game;
        
        StartCoroutine(SceneLoader.Instance.LoadScene(-1));
    }

    #endregion
    
    #region Door
    
    public void ShowDoor(bool show)
    {
        door.gameObject.SetActive(show);
    }
    public void OpenDoor()
    {
        door.Open(PlayerManager.Instance.transform.position);
    }
    public void CloseDoor()
    {
        door.Close();
    }

    #endregion
    
    #region Shop
    
    public void ShowShop(bool show)
    {
        shop.gameObject.SetActive(show);
    }

    #endregion
}
