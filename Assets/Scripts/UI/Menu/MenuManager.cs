using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    
    public static MenuManager Instance { get; private set; }

    [Header("Menu Managers")]
    public LoginMenuManager loginMenu;
    public MainMenuManager mainMenu;
    public LocalBattleMenuManager battleMenu;
    public InventoryMenuManager inventoryMenu;
    public StatisticsMenuManager statisticsMenu;
    public QuitMenuManager quitMenu;
    
    [Header("Properties")]
    public Enums.MenuType menuType;
    
    #endregion

    private void Awake()
    {
        Instance = this;
        
        menuType = Enums.MenuType.Login;

        loginMenu      = GameObject.FindObjectsOfType<LoginMenuManager>(true)[0];
        mainMenu       = GameObject.FindObjectsOfType<MainMenuManager>(true)[0];
        battleMenu     = GameObject.FindObjectsOfType<LocalBattleMenuManager>(true)[0];
        inventoryMenu  = GameObject.FindObjectsOfType<InventoryMenuManager>(true)[0];
        statisticsMenu = GameObject.FindObjectsOfType<StatisticsMenuManager>(true)[0];
        quitMenu       = GameObject.FindObjectsOfType<QuitMenuManager>(true)[0];

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
