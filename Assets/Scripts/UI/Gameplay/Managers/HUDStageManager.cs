using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDStageManager : MonoBehaviour
{
    #region Attibutes

    [Header("Interactables")]
    public CoinManager coin;
    public DialogueUIManager dialogue;

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
        
        canInteract = StageManager.Instance.door.isOpenable ||  StageManager.Instance.shop.isOpenable;
        interact.gameObject.SetActive(canInteract);
    }

    public void Initialize()
    {
        coin     = GetComponentInChildren<CoinManager>();
        dialogue = GameObject.FindObjectsOfType<DialogueUIManager>(true)[0];

        mask     = transform.GetChild(0);
        back     = transform.GetChild(2);
        interact = transform.GetChild(3);
        timer    = transform.GetChild(4);
        quit     = transform.GetChild(5);

        coin.Initialize();
        
        dialogue.SetManager(this);

        Button btnBack = back.GetComponent<Button>();
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDAction.Back); });
        
        Button btnInteract = interact.GetComponent<Button>();
        btnInteract.onClick.RemoveAllListeners();
        btnInteract.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDAction.Interact); });

        Button btnQuit = quit.GetChild(0).GetComponent<Button>();
        btnQuit.onClick.RemoveAllListeners();
        btnQuit.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDAction.Quit); });

        _initialized = true;
    }

    public void SwitchToBuff()
    {
        HUDManager.Instance.Action(Enums.HUDAction.SwitchBuff);
    }

    #region Coin

    public void AddCoins(bool hasReward, int amount = 0)
    {
        int reward = 0;
        if (hasReward)
            reward = timer.GetComponent<StageTimer>().GetRewardAmount();
        coin.AddCoins(reward + amount);
    }

    #endregion

    #region Dialogue

    public void ShowDialogue(bool visible)
    {
        dialogue.gameObject.SetActive(visible);
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
