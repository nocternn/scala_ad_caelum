using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenuManager : MonoBehaviour
{
    [SerializeField] private MenuManager _manager;

    public void SetManager(MenuManager manager)
    {
        _manager = manager;
    }

    public void Back()
    {
        _manager.menuType = Enums.MenuType.Main;

        _manager.quitMenu.gameObject.SetActive(false);
        _manager.mainMenu.gameObject.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
