using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private WeaponItem _currentWeapon;
    [SerializeField] private Tuple<bool, int, Enums.WeaponActionType> _lastSuccessfulAttackResult;
    [SerializeField] private Tuple<bool, int, Enums.WeaponActionType> _lastAttackResult;

    void Awake()
    {
        _currentWeapon = StatisticsManager.Instance.playerWeapon;
    }

    public void HandleWeaponCombo()
    {
        if (!PlayerManager.Instance.inputHandler.comboFlag)
            return;
        
        PlayerManager.Instance.animatorHandler.SetBool("canDoCombo", false);

        _lastAttackResult = _currentWeapon.PerformAttack(
                Enums.CharacterType.Playable,
                Enums.WeaponActionType.Basic,
                _lastSuccessfulAttackResult.Item2 + 1
            );
        UpdateLastestSuccess();
    }

	public void HandleActiveAttack()
    {
        // Check SP requirement
        float currentSP = PlayerManager.Instance.stats.currentSkillPoints;
        float costSP = _currentWeapon.GetSkillCost(Enums.WeaponActionType.Active);
        bool hasEnoughSP = currentSP >= costSP;
        // Check skill cooldown requirement
        bool canUseSkill = !_currentWeapon.GetSkillStatus(Enums.WeaponActionType.Active).Item1;

        // Allow skill usage if both requirements are met
        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= costSP;
            
            _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.WeaponActionType.Active);
            UpdateLastestSuccess();
        }
    }
    
    public void HandleBasicAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.WeaponActionType.Basic, 1);
        UpdateLastestSuccess();
    }
    public void HandleShootAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.WeaponActionType.Shoot);
        UpdateLastestSuccess();
    }

    public void HandleChargedAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.WeaponActionType.Charged);
        UpdateLastestSuccess();
    }

	public void HandleUltimateAttack()
    {
        // Check SP requirement
        float currentSP = PlayerManager.Instance.stats.currentSkillPoints;
        float costSP = _currentWeapon.GetSkillCost(Enums.WeaponActionType.Ultimate);
        bool hasEnoughSP = currentSP >= costSP;
        // Check skill cooldown requirement
        bool canUseSkill = !_currentWeapon.GetSkillStatus(Enums.WeaponActionType.Ultimate).Item1;

        // Allow skill usage if both requirements are met
        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= costSP;
            
            _lastAttackResult =
                _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.WeaponActionType.Ultimate);
            UpdateLastestSuccess();
        }
    }

	public bool IsAttackOfType(Enums.WeaponActionType type)
    {
        return _lastSuccessfulAttackResult.Item3 == type;
    }

    private void UpdateLastestSuccess()
    {
        if (_lastAttackResult.Item1)
            _lastSuccessfulAttackResult = _lastAttackResult;
    }
}
