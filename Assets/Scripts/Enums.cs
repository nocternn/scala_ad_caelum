using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum AmmoType
    {
        Arrow,
        Bolt,
        Bullet
    }
    
    public enum CardType
    {
        Bodhi,
        Decimation,
        Deliverance,
        Gold,
        Vicissitude
    }
    
    public enum StageType
    {
        Buff,
        Combat,
        Shop,
        Quit
    }
    
    public enum MenuType
    {
        Main,
        Inventory,
        Quit
    }

    public enum SceneType
    {
        Menu,
        Game
    }
}
