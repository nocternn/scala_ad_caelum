using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    
    public static MenuManager Instance { get; private set; }

    [Header("Menu Views")]
    public MenuViewLogin loginMenu;
    public MenuViewMain mainMenu;
    public MenuViewLocalBattle battleMenu;
    public MenuViewInventory inventoryMenu;
    public MenuViewStatistics statisticsMenu;
    public MenuViewQuit quitMenu;
    
    [Header("Properties")]
    public Enums.MenuType menuType;
    
    #endregion

    private void Awake()
    {
        Instance = this;
        
        menuType = Enums.MenuType.Login;

        loginMenu      = GameObject.FindObjectsOfType<MenuViewLogin>(true)[0];
        mainMenu       = GameObject.FindObjectsOfType<MenuViewMain>(true)[0];
        battleMenu     = GameObject.FindObjectsOfType<MenuViewLocalBattle>(true)[0];
        inventoryMenu  = GameObject.FindObjectsOfType<MenuViewInventory>(true)[0];
        statisticsMenu = GameObject.FindObjectsOfType<MenuViewStatistics>(true)[0];
        quitMenu       = GameObject.FindObjectsOfType<MenuViewQuit>(true)[0];

        SceneLoader.Instance.sceneType = Enums.SceneType.Menu;
    }

    public void Initialize()
    {
        ToggleAllMenus(false);

        switch (menuType)
        {
            case Enums.MenuType.Login:
                loginMenu.gameObject.SetActive(true);
                break;
            case Enums.MenuType.LocalBattle:
                battleMenu.gameObject.SetActive(true);
                break;
            case Enums.MenuType.Inventory:
                inventoryMenu.gameObject.SetActive(true);
                break;
            case Enums.MenuType.Statistics:
                statisticsMenu.gameObject.SetActive(true);
                break;
            case Enums.MenuType.Quit:
                quitMenu.gameObject.SetActive(true);
                break;
            default:
                mainMenu.gameObject.SetActive(true);
                break;
        }
    }

    public void Back()
    {
        menuType = Enums.MenuType.Main;
        Initialize();
    }

    private void ToggleAllMenus(bool visible)
    {
        loginMenu.gameObject.SetActive(visible);
        mainMenu.gameObject.SetActive(visible);
        battleMenu.gameObject.SetActive(visible);
        inventoryMenu.gameObject.SetActive(visible);
        statisticsMenu.gameObject.SetActive(visible);
        quitMenu.gameObject.SetActive(visible);
    }
}
