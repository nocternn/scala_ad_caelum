using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dictionaries
{
    public static Dictionary<Enums.EnemyType, string> EnemyType = new Dictionary<Enums.EnemyType, string>()
    {
        { Enums.EnemyType.LocalPlayer, "LocalPlayer" },
        { Enums.EnemyType.Archer, "Archer" },
        { Enums.EnemyType.Spearwoman, "Spearwoman" },
        { Enums.EnemyType.Crossbowman, "Crossbowman" },
        { Enums.EnemyType.Ninja, "Ninja" },
        { Enums.EnemyType.Paladin, "Paladin" },
    };
    
    public static Dictionary<Enums.WeaponType, string> WeaponTypePlayer = new Dictionary<Enums.WeaponType, string>()
    {
        { Enums.WeaponType.Greatsword, "Greatsword" },
        { Enums.WeaponType.Gauntlet, "Gauntlet" },
        { Enums.WeaponType.Katana, "Katana" },
        { Enums.WeaponType.Pistol, "Pistol" },
        { Enums.WeaponType.Scythe, "Scythe" },
    };
    
    public static Dictionary<Enums.WeaponType, string> WeaponTypeEnemy = new Dictionary<Enums.WeaponType, string>()
    {
        { Enums.WeaponType.Bow, "Bow" },
        { Enums.WeaponType.Crossbow, "Crossbow" },
        { Enums.WeaponType.Katana, "Katana" },
        { Enums.WeaponType.Spear, "Spear" },
        { Enums.WeaponType.Sword, "Sword" },
    };
}
