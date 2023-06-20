using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : CharacterLocomotion
{
    private EnemyManager _manager;

    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }
    
    public void Initialize()
    {
        characterCollider = _manager.transform.GetComponent<CapsuleCollider>();
        characterColliderBlocker = _manager.transform.Find("CombatColliders")
            .transform.Find("CharacterColliderBlocker").GetComponent<CapsuleCollider>();

        DisableCharacterCollision();
    }
}
