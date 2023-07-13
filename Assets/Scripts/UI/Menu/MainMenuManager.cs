using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneLoader.Instance.sceneType = Enums.SceneType.Game;
        SceneLoader.Instance.previousSceneType = Enums.SceneType.Menu;

        if (StatisticsManager.Instance.IsBeginning())
            StatisticsManager.Instance.ResetStatsPlayer(Enums.StatsType.Combat);

        StartCoroutine(SceneLoader.Instance.LoadScene(1));
    }

    public void LocalBattle()
    {
        MenuManager.Instance.menuType = Enums.MenuType.LocalBattle;
        MenuManager.Instance.Initialize();
    }

    public void Inventory()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Inventory;
        MenuManager.Instance.Initialize();
    }

	public void Statistics()
	{
        MenuManager.Instance.menuType = Enums.MenuType.Statistics;
        MenuManager.Instance.Initialize();
        MenuManager.Instance.statisticsMenu.SetValues();
	}

    public void Quit()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Quit;
        MenuManager.Instance.Initialize();
    }
}
