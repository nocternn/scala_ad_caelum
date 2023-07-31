using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewLocalPlayer : CardView
{
    public Statistics playerStats;
    public WeaponItem playerWeapon;

    public override void Awake()
    {
        base.Awake();
    }
    
    public void UpdateUI(Statistics stats)
    {
        _title.text = stats.name;
        _description.text = stats.GetDescription();

        playerStats = stats;
        playerWeapon = MenuManager.Instance.inventoryMenu.GetWeapon(stats.weapon);
    }
}
