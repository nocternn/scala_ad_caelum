using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWeaponController : CardController
{
    public WeaponItem weapon;

    public override void Awake()
    {
        base.Awake();
    }
    
    public void UpdateUI(WeaponItem weaponItem)
    {
        _icon.sprite = weaponItem.primaryIcon;
        _title.text = weaponItem.GetName();
        _description.text = weaponItem.GetDescription();

        weapon = weaponItem;
    }
}
