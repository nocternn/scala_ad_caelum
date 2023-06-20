using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Attributes

    public static StageManager Instance { get; private set; }

    private bool _initialized;
    private int _clearReward;
    
    public Enums.StageType stageType;
    public Enums.StageType previousStageType;
    
    public int id = 1;
    public int coinsAvailable;
    
    public SceneLoader sceneLoader;

    public PlayerManager player;
    public EnemyManager enemy;
    
    public HUDManager hud;
    public new CameraHandler camera;

    public List<CardItem> selectedBuffs;
    public GameObject buffHolder;

    #endregion

    private void Start()
    {
        _clearReward = 100;
        _initialized = false;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    private void Update()
    {
        if (!_initialized)
            return;
        
        if (stageType != Enums.StageType.Combat)
            return;
    
        foreach (CardItem buff in selectedBuffs)
        {
            buff.effect.Apply(buff, player, enemy);
        }
    }

    private void Reset()
    {
        _clearReward = 100;
        _initialized = false;

        stageType = Enums.StageType.Buff;
        previousStageType = Enums.StageType.Buff;

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

    private void TogglePlayer(bool show)
    {
        if (show)
        {
            player.gameObject.SetActive(true);
            player.SetManager(this);
            player.Initialize();
            player.SetWeapon(sceneLoader.weapon);
        }
        else
        {
            player.gameObject.SetActive(false);
        }
    }

    private void ToggleEnemy(bool show)
    {
        if (show)
        {
            enemy.gameObject.SetActive(true);
            enemy.SetManager(this);
            enemy.Initialize();
            enemy.SetEnemyType(EnemyManager.Types[id - 1]);
        }
        else
        {
            
        }
    }
    
    public void Initialize()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        camera = GameObject.Find("Camera Holder").GetComponent<CameraHandler>();
        hud    = GameObject.Find("HUD").GetComponent<HUDManager>();

        hud.SetManager(this);
        hud.Initialize();
        
        _initialized = true;
    }

    public void SwitchToCombat(CardItem selectedBuff)
    {
        selectedBuff.EnableEffect(buffHolder);
        selectedBuffs.Add(selectedBuff);

        player = GameObject.FindObjectsOfType<PlayerManager>(true)[0];
        enemy = GameObject.FindObjectsOfType<EnemyManager>(true)[0];

        TogglePlayer(true);
        ToggleEnemy(true);
        
        previousStageType = stageType;
        stageType = Enums.StageType.Combat;
        
        hud.Initialize();
    }

    public void SwitchToShop()
    {
        previousStageType = stageType;
        stageType = Enums.StageType.Shop;

        TogglePlayer(false);
        
        hud.Initialize();
    }

    public void SwitchToNextStage()
    {
        id++;
        _initialized = false;

        hud.hudStage.OpenDoor();

        if (id < 5)
        {
            stageType = Enums.StageType.Buff;
            previousStageType = Enums.StageType.Buff;
            
            StartCoroutine(sceneLoader.LoadScene(0));
        }
        else
        {
            Quit();
        }
    }

    public void EndStageWin()
    {
        hud.hudStage.ToggleTimer(false);
        hud.hudStage.CloseDoor();
        hud.hudStage.ShowDoor(true);

        if (id % 2 == 0)
            hud.hudStage.ShowShop(true);

        // Lock off target
        player.inputHandler.lockOnInput = true;
        player.inputHandler.lockOnFlag = true;
        player.inputHandler.HandleLockOnInput();

        // Spawn coin rewards
        hud.hudStage.coin.Spawn(10);
        hud.hudStage.AddCoins(true, _clearReward);
        coinsAvailable = hud.hudStage.coin.GetCoins();
    }

    public void EndStageLoss()
    {
        Quit();
    }

    public void Interact()
    {
        if (hud.hudStage.door.isOpenable)
        {
            SwitchToNextStage();
        }
        else if (hud.hudStage.shop.isOpenable)
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

            hud.hudStage.ShowDoor(true);
            hud.hudStage.ShowShop(true);
            
            hud.shop.gameObject.SetActive(false);
        }
        else if (stageType == Enums.StageType.Quit)
        {
            stageType = previousStageType;
            previousStageType = Enums.StageType.Quit;

            if (stageType == Enums.StageType.Combat)
            {
                TogglePlayer(true);

                if (enemy != null)
                {
                    hud.hudStage.ToggleTimer(true);

                    ToggleEnemy(true);
                }
            }
            
            hud.hudStage.ShowQuitConfirmation(false);
        }
        else
        {
            previousStageType = stageType;
            stageType = Enums.StageType.Quit;

            if (previousStageType == Enums.StageType.Combat)
            {
                hud.hudStage.ToggleTimer(false);

                TogglePlayer(false);

                if (enemy != null)
                    ToggleEnemy(false);
            }
            
            hud.hudStage.ShowQuitConfirmation(true);
        }
    }

    public void Quit()
    {
        Reset();
        
        sceneLoader.sceneType = Enums.SceneType.Menu;
        StartCoroutine(sceneLoader.LoadScene(-1));
    }
}
