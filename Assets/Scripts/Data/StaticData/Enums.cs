using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enums
{
    public enum AmmoType
    {
        Arrow,
        Bolt,
        Bullet
    }

    public enum CameraType
    {
        Aim,
        Free,
        Locked,
        Standard
    }
    
    public enum CharacterType
    {
        Boss,
        Playable,
        Standard
    }
    public enum CharacterActionType
    {
        // Null
        None,
        // Movement
        Aim,
        Block,
        Dodge,
        Parry
    }
    public enum CharacterInteractionType
    {
        OpenDoor,
        CloseDoor,
        OpenShop,
        CloseShop
    }
    
    public enum EffectType
    {
        Bodhi,
        Decimation,
        Deliverance,
        Gold,
        Vicissitude
    }
    
    public enum EnemyType
    {
        LocalPlayer,
        Archer,
        Spearwoman,
        Crossbowman,
        Ninja,
        Paladin
    }
    
    public enum HUDType
    {
        Combat,
        Dialogue,
        Effects,
        Shop,
        Quit
    }
    public enum HUDActionType
    {
        Back,
        Interact,
        Quit,
        SwitchEffects,
        SwitchCombat
    }
    
    public enum WeaponType
    {
        // Player weapons
        Greatsword,
        Gauntlet,
        Pistol,
        Scythe,
        // Enemy weapons
        Bow,
        Crossbow,
        Spear,
        Sword,
        Shield,
        // Common wweapons
        Katana
    }
    public enum WeaponActionType
    {
        // Null
        None,
        // Attack
        Basic,
        Charged,
        Active,
        Ultimate,
        Shoot,
        Combo,
    }
    public enum WeaponCombatType
    {
        Melee,
        Ranged
    }
    public enum WeaponHandlingType
    {
        Left,
        Right,
        Both
    }
    
    public enum MenuType
    {
        Inventory,
        LocalBattle,
        Login,
        Main,
        Quit,
        Statistics
    }

    public enum SceneType
    {
        Menu,
        Game
    }
    
    public enum StatsType
    {
        Progress,
        Combat,
        Meta
    }
}
