using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuViewLocalBattle : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    private void Awake()
    {
        _container = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);

        Statistics[] allStats = StatisticsManager.Instance.GetAllStats();
        Array.Sort(allStats, new StatisticsComparer());
        Array.Reverse(allStats);

        foreach (var stats in allStats)
        {
            if (stats.name.Equals(StatisticsManager.Instance.playerStats.name))
                continue;

            CardViewLocalPlayer card = Instantiate(_prefab, _container.transform).GetComponent<CardViewLocalPlayer>();
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
                CardViewLocalPlayer selectedCard = child.GetComponent<CardViewLocalPlayer>();
                selectedCard.transform.GetComponent<Toggle>().isOn = false;

                // Register opponent
                StatisticsManager.Instance.opponentStats = selectedCard.playerStats;
                StatisticsManager.Instance.opponentWeapon = selectedCard.playerWeapon;

                break;
            }
        }

        PlayLocalBattle();
    }

    public void Randomize()
    {
        Transform child = _container.GetChild(UnityEngine.Random.Range(0, _container.childCount - 1));

        CardViewLocalPlayer selectedCard = child.GetComponent<CardViewLocalPlayer>();

        // Register opponent
        StatisticsManager.Instance.opponentStats = selectedCard.playerStats;
        StatisticsManager.Instance.opponentWeapon = selectedCard.playerWeapon;

        PlayLocalBattle();
    }
    
    private void PlayLocalBattle()
    {
        SceneLoader.Instance.sceneType = Enums.SceneType.Game;
        SceneLoader.Instance.previousSceneType = Enums.SceneType.Menu;
        SceneLoader.Instance.previousMenuType = Enums.MenuType.LocalBattle;
        
        StartCoroutine(SceneLoader.Instance.LoadScene(2));
    }
}
