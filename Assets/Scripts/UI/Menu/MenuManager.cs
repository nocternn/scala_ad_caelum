using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    
    public static MenuManager Instance { get; private set; }

    [Header("Menu Managers")]
    public MainMenuManager mainMenu;
    public InventoryMenuManager inventoryMenu;
    public QuitMenuManager quitMenu;
    
    [Header("Properties")]
    public Enums.MenuType menuType;
    
    #endregion

    private void Awake()
    {
        Instance = this;
        
        menuType = Enums.MenuType.Main;

        mainMenu      = GameObject.FindObjectsOfType<MainMenuManager>(true)[0];
        inventoryMenu = GameObject.FindObjectsOfType<InventoryMenuManager>(true)[0];
        quitMenu      = GameObject.FindObjectsOfType<QuitMenuManager>(true)[0];

        SceneLoader.Instance.sceneType = Enums.SceneType.Menu;
    }
}
