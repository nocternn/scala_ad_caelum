using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private MenuManager _manager;

    public void SetManager(MenuManager manager)
    {
        _manager = manager;
    }

    public void Play()
    {
        if (_manager.sceneLoader.weapon == null)
            _manager.sceneLoader.weapon = _manager.inventoryMenu.weapons[0];
        
        _manager.sceneLoader.sceneType = Enums.SceneType.Game;
        StartCoroutine(_manager.sceneLoader.LoadScene(1));
    }

    public void Inventory()
    {
        _manager.menuType = Enums.MenuType.Inventory;

        _manager.mainMenu.gameObject.SetActive(false);
        _manager.inventoryMenu.gameObject.SetActive(true);
    }

    public void Quit()
    {
        _manager.menuType = Enums.MenuType.Quit;

        _manager.mainMenu.gameObject.SetActive(false);
        _manager.quitMenu.gameObject.SetActive(true);
    }
}
