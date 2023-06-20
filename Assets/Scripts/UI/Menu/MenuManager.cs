using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Attributes
    
    public SceneLoader sceneLoader;
    public Enums.MenuType menuType;

    [Header("Menu Managers")]
    public MainMenuManager mainMenu;
    public InventoryMenuManager inventoryMenu;
    public QuitMenuManager quitMenu;
    
    #endregion

    private void Awake()
    {
        menuType = Enums.MenuType.Main;

        mainMenu      = GameObject.FindObjectsOfType<MainMenuManager>(true)[0];
        inventoryMenu = GameObject.FindObjectsOfType<InventoryMenuManager>(true)[0];
        quitMenu      = GameObject.FindObjectsOfType<QuitMenuManager>(true)[0];

        mainMenu.SetManager(this);
        inventoryMenu.SetManager(this);
        quitMenu.SetManager(this);
        
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        sceneLoader.sceneType = Enums.SceneType.Menu;
    }
}
