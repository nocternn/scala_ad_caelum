using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public float movementSpeed = 5; 
    public float rotationSpeed = 10;
    
    protected CapsuleCollider characterCollider;
    protected CapsuleCollider characterColliderBlocker;

    public void DisableCharacterCollision()
    {
        Physics.IgnoreCollision(characterCollider, characterColliderBlocker, true);
    }
}
