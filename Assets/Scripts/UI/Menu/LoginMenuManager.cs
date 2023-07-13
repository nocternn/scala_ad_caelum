using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _txtPlayerName;

    private void Awake()
    {
        _txtPlayerName = transform.GetChild(0).GetComponent<TMP_InputField>();
    }
    
    public void Login()
    {
        bool hasRead = StatisticsManager.Instance.ReadStatsPlayer(_txtPlayerName.text);
        if (!hasRead)
        {
            StatisticsManager.Instance.ReadStatsPlayer();
            StatisticsManager.Instance.playerStats.name = _txtPlayerName.text;
            StatisticsManager.Instance.WriteStatsPlayer();
        }
        
        MenuManager.Instance.Back();
    }
}
