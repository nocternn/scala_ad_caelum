using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDViewStage : MonoBehaviour
{
    #region Attibutes

    [Header("Interactables")]
    public CurrencyView currency;
    public DialogueView dialogue;

    [Header("Progress")]
    public ProgressBarView progressBar;

    [Header("Transforms")]
    public Transform mask;
    public Transform back;
    public Transform interact;
    public Transform report;
    public Transform timer;
    public Transform quit;

    [Header("Properties")]
    public bool canInteract;
    [SerializeField] private bool _initialized;

    #endregion

    void Awake()
    {
        _initialized = false;
    }

    void Update()
    {
        if (!_initialized)
            return;

        canInteract = StageManager.Instance.door.IsOpenable() ||  StageManager.Instance.shop.IsOpenable();
        interact.gameObject.SetActive(canInteract);
    }

    public void Initialize()
    {
        if (!StageManager.Instance.isLocalBattle)
        {
            progressBar = GetComponentInChildren<ProgressBarView>();
            currency    = GetComponentInChildren<CurrencyView>();
            dialogue    = GameObject.FindObjectsOfType<DialogueView>(true)[0];
        }

        mask     = transform.GetChild(0);
        back     = transform.GetChild(1);
        interact = transform.GetChild(2);
        report   = transform.GetChild(3);
        timer    = transform.GetChild(4);
        quit     = transform.GetChild(5);

        if (!StageManager.Instance.isLocalBattle)
        {
            progressBar.UpdateProgress(StageManager.Instance.id - 1);
            currency.Initialize();
            dialogue.SetManager(this);
        }

        Button btnBack = back.GetComponent<Button>();
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDActionType.Back); });

        Button btnInteract = interact.GetComponent<Button>();
        btnInteract.onClick.RemoveAllListeners();
        btnInteract.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDActionType.Interact); });

        Button btnQuit = quit.GetChild(0).GetComponent<Button>();
        btnQuit.onClick.RemoveAllListeners();
        btnQuit.onClick.AddListener(delegate { HUDManager.Instance.Action(Enums.HUDActionType.Quit); });

        _initialized = true;
    }

    public void SwitchToBuff()
    {
        HUDManager.Instance.Action(Enums.HUDActionType.SwitchEffects);
    }

    #region Coin

    public void AddCoins(bool hasReward, int amount = 0)
    {
        int reward = 0;
        if (hasReward)
            reward = timer.GetComponent<StageTimerView>().GetRewardAmount();
        currency.AddCoins(reward + amount);
    }

    #endregion

    #region Dialogue

    public void ShowDialogue(bool visible)
    {
        dialogue.gameObject.SetActive(visible);
    }

    #endregion

    #region Report

    public void ShowCombatReport(bool visible, bool currencysVisible = true)
    {
        if (report.gameObject.activeSelf != visible)
        {
            report.GetComponent<CombatReportView>().ToggleCoins(currencysVisible);
            report.gameObject.SetActive(visible);
        }
    }

    public void InitializeCombatReport(int clearReward)
    {
        CombatReportView reportView = report.GetComponent<CombatReportView>();
        StageTimerView timerView = timer.GetComponent<StageTimerView>();

        int timeElapsed = timerView.GetElapsed();
        int timeReward  = timerView.GetRewardAmount();

        reportView.Initialize();
        reportView.SetTime(timeElapsed);
        reportView.SetCoins(clearReward + timeReward);
    }

    #endregion

    #region Timer

    public void ToggleTimer(bool isRunning)
    {
        timer.GetComponent<StageTimerView>().pause = !isRunning;
    }

    #endregion

    #region Quit

    public void ShowQuitConfirmation(bool show)
    {
        if (show) Cursor.lockState = CursorLockMode.Confined;
        mask.gameObject.SetActive(show);
        quit.gameObject.SetActive(show);
    }

    #endregion
}
