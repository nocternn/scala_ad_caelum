using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    #region Attributes

    public GameObject model;
    public AmmoItem ammo;
    public Sprite secondaryIcon;
    
    public int id;

    [Header("Stats")]
    public int atk;
    public int crt;

    [Header("Descriptions")]
    public string descriptionActive;
    public string descriptionUltimate;

    [Header("Skill Info")]
    public WeaponItemSkillActive skillActive; 
    public WeaponItemSkillUltimate skillUltimate; 
    public int activeCost;
    public int activeCooldown;
    public int ultimateCost;
    public int ultimateCooldown;
    
    [Header("Active Attack Animations")]
    public string activeAttack;
    
    [Header("Basic Attack Animations")]
    public string basicAttack01;
    public string basicAttack02;
    public string basicAttack03;
    public string basicAttack04;
    
    [Header("Charged Attack Animations")]
    public string chargedAttack;
    
    [Header("Ultimate Attack Animations")]
    public string ultimateAttack;

    #endregion

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        string updatedDescription = description;
        updatedDescription += "\nActive: " + descriptionActive;
        updatedDescription += "\nUltimate: " + descriptionUltimate;

        return updatedDescription;
    }

    public void SetSkills(GameObject holder)
    {
        foreach (var comp in holder.GetComponents<Component>())
        {
            if (!(comp is Transform))
            {
                Destroy(comp);
            }
        }

        skillActive = holder.AddComponent<WeaponItemSkillActive>() as WeaponItemSkillActive;
        skillUltimate = holder.AddComponent<WeaponItemSkillUltimate>() as WeaponItemSkillUltimate;

        skillActive.Initialize(this);
        skillUltimate.Initialize(this);
    }
}
