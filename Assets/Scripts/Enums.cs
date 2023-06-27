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
        Dialogue,
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

    public enum HUDAction
    {
        Back,
        Interact,
        Quit,
        SwitchBuff,
        SwitchCombat
    }
    
    public enum CameraType
    {
        Aim,
        Free,
        Locked,
        Standard
    }
}
