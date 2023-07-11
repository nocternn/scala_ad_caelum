using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenuManager : MonoBehaviour
{
    public void Back()
    {
        MenuManager.Instance.menuType = Enums.MenuType.Main;

        MenuManager.Instance.ToggleAllMenus(false);
        MenuManager.Instance.mainMenu.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
