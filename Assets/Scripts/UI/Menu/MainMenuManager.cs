using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        if (SceneLoader.Instance.playerWeapon == null)
            SceneLoader.Instance.playerWeapon = MenuManager.Instance.inventoryMenu.weapons[0];
        
        if (SceneLoader.Instance.statsManager.IsBeginning())
            SceneLoader.Instance.statsManager.ResetStatsPlayer(SceneLoader.Instance.playerName, Enums.StatsType.Combat);
        
        SceneLoader.Instance.sceneType = Enums.SceneType.Game;
        SceneLoader.Instance.previousSceneType = Enums.SceneType.Menu;
        
        StartCoroutine(SceneLoader.Instance.LoadScene(1));
    }

    public void Inventory()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Inventory;

        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.inventoryMenu.gameObject.SetActive(true);
    }

    public void LocalBattle()
    {
        MenuManager.Instance.menuType = Enums.MenuType.LocalBattle;
        
        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.battleMenu.gameObject.SetActive(true);
    }

	public void Statistics()
	{
        MenuManager.Instance.menuType = Enums.MenuType.Statistics;

        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.statisticsMenu.gameObject.SetActive(true);
        MenuManager.Instance.statisticsMenu.SetValues();
	}

    public void Quit()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Quit;

        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.quitMenu.gameObject.SetActive(true);
    }
}
