using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public GameObject model;

    [Header("Skill Info")]
    public int active_cost;
    public int ultimate_cost;
    
    [Header("Active Attack Animations")]
    public string active_attack;
    
    [Header("Basic Attack Animations")]
    public string basic_attack_01;
    public string basic_attack_02;
    public string basic_attack_03;
    public string basic_attack_04;
    
    [Header("Charged Attack Animations")]
    public string charged_attack;
    
    [Header("Ultimate Attack Animations")]
    public string ultimate_attack;
}
