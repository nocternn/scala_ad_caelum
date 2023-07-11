using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtRuns;
    [SerializeField] private TMP_Text _txtStages;
    [SerializeField] private TMP_Text _txtDeaths;
    [SerializeField] private TMP_Text _txtDamageDealt;
    [SerializeField] private TMP_Text _txtDamageReceived;
    [SerializeField] private TMP_Text _txtDamageSingleHit;
    
    private void Awake()
    {
        Transform container = transform.GetChild(2).GetChild(0).GetChild(0);
        _txtRuns            = container.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        _txtStages          = container.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _txtDeaths          = container.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageDealt     = container.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageReceived  = container.GetChild(4).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageSingleHit = container.GetChild(5).GetChild(1).GetComponent<TMP_Text>();
    }

    public void Initialize()
    {
        bool readSuccess = SceneLoader.Instance.statsManager.ReadStats();
        if (!readSuccess)
            return;
        
        _txtRuns.text            = SceneLoader.Instance.statsManager.stats.numberOfRuns.ToString();
        _txtStages.text          = SceneLoader.Instance.statsManager.stats.numberOfStagesCleared.ToString();
        _txtDeaths.text          = SceneLoader.Instance.statsManager.stats.numberOfDeaths.ToString();
        _txtDamageDealt.text     = SceneLoader.Instance.statsManager.stats.totalDamageDealt.ToString();
        _txtDamageReceived.text  = SceneLoader.Instance.statsManager.stats.totalDamageReceived.ToString();
        _txtDamageSingleHit.text = SceneLoader.Instance.statsManager.stats.maxDamageSingleHit.ToString();
    }
    
    public void Back()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Main;

        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.mainMenu.gameObject.SetActive(true);
    }
}
