using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotion : CharacterLocomotion
{
    public void Initialize()
    {
        characterCollider = transform.GetComponent<CapsuleCollider>();
        characterColliderBlocker = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<CapsuleCollider>();

        DisableCharacterCollision();
    }
}
