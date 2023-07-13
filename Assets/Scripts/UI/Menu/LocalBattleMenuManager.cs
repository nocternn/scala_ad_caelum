using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalBattleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    private void Awake()
    {
        _container = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);

        Statistics[] allStats = StatisticsManager.Instance.GetAllStats();
        foreach (var stats in allStats)
        {
            if (stats.name.Equals(StatisticsManager.Instance.playerStats.name))
                continue;

            CardLocalPlayerController card = Instantiate(_prefab, _container.transform).GetComponent<CardLocalPlayerController>();
            card.UpdateUI(stats);
            card.transform.GetComponent<Toggle>().group = _container.transform.GetComponent<ToggleGroup>();
        }
    }

    public void Choose()
    {
        foreach (Transform child in _container)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                CardLocalPlayerController selectedCard = child.GetComponent<CardLocalPlayerController>();
                selectedCard.transform.GetComponent<Toggle>().isOn = false;

                // Register opponent
                StatisticsManager.Instance.opponentStats = selectedCard.playerStats;
                StatisticsManager.Instance.opponentWeapon = selectedCard.playerWeapon;

                break;
            }
        }

        MenuManager.Instance.Back();
    }

    public void Randomize()
    {
        Transform child = _container.GetChild(Random.Range(0, _container.childCount - 1));

        CardLocalPlayerController selectedCard = child.GetComponent<CardLocalPlayerController>();

        // Register opponent
        StatisticsManager.Instance.opponentStats = selectedCard.playerStats;
        StatisticsManager.Instance.opponentWeapon = selectedCard.playerWeapon;

        MenuManager.Instance.Back();
    }
}
