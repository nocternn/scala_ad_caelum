using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDStageManager : MonoBehaviour
{
    #region Attibutes
    
    [SerializeField] private StageManager _stage;
        
    [Header("Interactables")]
    public CoinManager coin;
    public DialogueManager dialogue;
    public Door door;
    public Shop shop;

    [Header("Transforms")]
    public Transform mask;
    public Transform back;
    public Transform interact;
    public Transform timer;
    public Transform quit;

    [Header("Properties")]
    public bool canInteract;
    [SerializeField] private bool _initialized;

    #endregion

    void Start()
    {
        _initialized = false;
    }

    void Update()
    {
        if (!_initialized)
            return;
        
        canInteract = door.isOpenable || shop.isOpenable;
        interact.gameObject.SetActive(canInteract);
    }

    public void Initialize()
    {
        coin     = GetComponentInChildren<CoinManager>();
        dialogue = GetComponentInChildren<DialogueManager>();
        door     = GameObject.FindObjectsOfType<Door>(true)[0];
        shop     = GameObject.FindObjectsOfType<Shop>(true)[0];

        mask     = transform.GetChild(0);
        back     = transform.GetChild(2);
        interact = transform.GetChild(3);
        timer    = transform.GetChild(4);
        quit     = transform.GetChild(5);

        coin.Initialize();
        dialogue.Initialize();
        door.Initialize();
        shop.Initialize();
        
        dialogue.SetManager(_stage);

        ShowDoor(false);
        ShowShop(false);

        Button btnBack = back.GetComponent<Button>();
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(_stage.Back);
        
        Button btnInteract = interact.GetComponent<Button>();
        btnInteract.onClick.RemoveAllListeners();
        btnInteract.onClick.AddListener(_stage.Interact);

        Button btnQuit = quit.GetChild(0).GetComponent<Button>();
        btnQuit.onClick.RemoveAllListeners();
        btnQuit.onClick.AddListener(_stage.Quit);

        _initialized = true;
    }
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }

    public void AddCoins(bool hasReward, int amount = 0)
    {
        int reward = 0;
        if (hasReward)
            reward = timer.GetComponent<StageTimer>().GetRewardAmount();
        coin.AddCoins(reward + amount);
    }

    #region Door
    
    public void ShowDoor(bool show)
    {
        door.gameObject.SetActive(show);
    }
    public void OpenDoor()
    {
        door.Open(_stage.player.transform.position);
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

    #region Timer

    public void ToggleTimer(bool isRunning)
    {
        timer.GetComponent<StageTimer>().pause = !isRunning;
    }

    #endregion

    #region Quit
    
    public void ShowQuitConfirmation(bool show)
    {
        mask.gameObject.SetActive(show);
        quit.gameObject.SetActive(show);
    }

    #endregion
}
