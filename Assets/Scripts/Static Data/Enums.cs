using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enums
{
    public enum CharacterType
    {
        Boss,
        Playable,
        Standard
    }
    
    public enum EnemyType
    {
        Archer,
        Spearwoman,
        Crossbowman,
        Ninja,
        Paladin
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

    public enum WeaponCombatType
    {
        Melee,
        Ranged
    }
    
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
        Statistics,
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

	public enum ActionType
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
        // Movement
		Aim,
		Block,
		Dodge,
		Parry
	}
}
