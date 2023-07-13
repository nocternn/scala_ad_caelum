using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private WeaponItem _currentWeapon;
    [SerializeField] private Tuple<bool, int, Enums.ActionType> _lastSuccessfulAttackResult;
    [SerializeField] private Tuple<bool, int, Enums.ActionType> _lastAttackResult;

    void Awake()
    {
        _currentWeapon = SceneLoader.Instance.playerWeapon;
    }

    public void HandleWeaponCombo()
    {
        if (!PlayerManager.Instance.inputHandler.comboFlag)
            return;
        
        PlayerManager.Instance.animatorHandler.SetBool("canDoCombo", false);

        _lastAttackResult = _currentWeapon.PerformAttack(
                Enums.CharacterType.Playable,
                Enums.ActionType.Basic,
                _lastSuccessfulAttackResult.Item2 + 1
            );
        UpdateLastestSuccess();
    }

	public void HandleActiveAttack()
    {
        // Check SP requirement
        float currentSP = PlayerManager.Instance.stats.currentSkillPoints;
        float costSP = _currentWeapon.GetSkillCost(Enums.ActionType.Active);
        bool hasEnoughSP = currentSP >= costSP;
        // Check skill cooldown requirement
        bool canUseSkill = !_currentWeapon.GetSkillStatus(Enums.ActionType.Active).Item1;

        // Allow skill usage if both requirements are met
        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= costSP;
            
            _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.ActionType.Active);
            UpdateLastestSuccess();
        }
    }
    
    public void HandleBasicAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.ActionType.Basic, 1);
        UpdateLastestSuccess();
    }
    public void HandleShootAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.ActionType.Shoot);
        UpdateLastestSuccess();
    }

    public void HandleChargedAttack()
    {
        _lastAttackResult = _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.ActionType.Charged);
        UpdateLastestSuccess();
    }

	public void HandleUltimateAttack()
    {
        // Check SP requirement
        float currentSP = PlayerManager.Instance.stats.currentSkillPoints;
        float costSP = _currentWeapon.GetSkillCost(Enums.ActionType.Ultimate);
        bool hasEnoughSP = currentSP >= costSP;
        // Check skill cooldown requirement
        bool canUseSkill = !_currentWeapon.GetSkillStatus(Enums.ActionType.Ultimate).Item1;

        // Allow skill usage if both requirements are met
        if (hasEnoughSP && canUseSkill)
        {
            PlayerManager.Instance.stats.currentSkillPoints -= costSP;
            
            _lastAttackResult =
                _currentWeapon.PerformAttack(Enums.CharacterType.Playable, Enums.ActionType.Ultimate, 0, false);
            UpdateLastestSuccess();
        }
    }

	public bool IsAttackOfType(Enums.ActionType type)
    {
        return _lastSuccessfulAttackResult.Item3 == type;
    }

    private void UpdateLastestSuccess()
    {
        if (_lastAttackResult.Item1)
            _lastSuccessfulAttackResult = _lastAttackResult;
    }
}
