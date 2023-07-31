using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuViewStatistics : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtLocalBattles;
    [SerializeField] private TMP_Text _txtStages;
    [SerializeField] private TMP_Text _txtRuns;
    [SerializeField] private TMP_Text _txtDeaths;
    [SerializeField] private TMP_Text _txtDamageDealt;
    [SerializeField] private TMP_Text _txtDamageReceived;
    [SerializeField] private TMP_Text _txtDamageSingleHit;

    private void Awake()
    {
        Transform container = transform.GetChild(3).GetChild(0).GetChild(0);
        _txtLocalBattles    = container.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        _txtStages          = container.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        _txtRuns            = container.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _txtDeaths          = container.GetChild(3).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageDealt     = container.GetChild(4).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageReceived  = container.GetChild(5).GetChild(1).GetComponent<TMP_Text>();
        _txtDamageSingleHit = container.GetChild(6).GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetValues()
    {
        _txtLocalBattles.text    = StatisticsManager.Instance.playerStats.meta.numberOfLocalBattlesWon.ToString();
        _txtStages.text          = StatisticsManager.Instance.playerStats.meta.numberOfStagesCleared.ToString();
        _txtRuns.text            = StatisticsManager.Instance.playerStats.meta.numberOfRuns.ToString();
        _txtDeaths.text          = StatisticsManager.Instance.playerStats.meta.numberOfDeaths.ToString();
        _txtDamageDealt.text     = StatisticsManager.Instance.playerStats.meta.totalDamageDealt.ToString();
        _txtDamageReceived.text  = StatisticsManager.Instance.playerStats.meta.totalDamageReceived.ToString();
        _txtDamageSingleHit.text = StatisticsManager.Instance.playerStats.meta.maxDamageSingleHit.ToString();
    }
}
