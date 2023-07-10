using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Active Attack")]
public class ActiveAttackAction : WeaponAction
{
    [Header("Specific Properties")]
    public WeaponItemSkillActive skill;
    public string description;
    public int cost;
    public int cooldown;

    protected override void Awake()
    {
        skill = GameObject.Find("SkillsHolder").GetComponent<WeaponItemSkillActive>();
        skill.Initialize(cooldown);

        type = Enums.ActionType.Active;
    }
    
    public override void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        skill.UseSkill(player.weaponSlotManager.GetCurrentWeapon().type);
        skill.Cooldown();
        
        if (playAnimation)
            player.PlayTargetAnimation(_animation, true);

        base.PerformAction(player, playAnimation);
    }

    public override string GetDescription()
    {
        return "Active: " + description;
    }
}
