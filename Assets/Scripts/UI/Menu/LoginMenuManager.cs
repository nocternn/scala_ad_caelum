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
        SceneLoader.Instance.playerName = _txtPlayerName.text;

        bool hasRead = SceneLoader.Instance.statsManager.ReadStatsPlayer(_txtPlayerName.text);
        if (!hasRead)
        {
            SceneLoader.Instance.statsManager.ReadStatsPlayer("default");
            SceneLoader.Instance.statsManager.WriteStatsPlayer(_txtPlayerName.text);
        }
        
        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.mainMenu.gameObject.SetActive(true);
    }
}
