using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Weapon Actions/Ultimate Attack")]
public class UltimateAttackAction : WeaponAction
{
    [Header("Specific Properties")]
    public WeaponSkillUltimate skill; 
    public string description;
    public int cost;
    public int cooldown;
    
    protected override void Awake()
    {
        type = Enums.WeaponActionType.Ultimate;
    }

    public override void Initialize()
    {
        skill = GameObject.Find("SkillsHolder").GetComponent<WeaponSkillUltimate>();
        skill.Initialize(cooldown);
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
        return "Ultimate: " + description;
    }
}
