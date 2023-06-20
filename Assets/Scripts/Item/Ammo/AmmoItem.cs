using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class AmmoItem : Item
{
    public GameObject model;
    
    [Header("Type")]
    public Enums.AmmoType type;

    [Header("Physics")]
    public float forwardVelocity = 550;
    public float upwardVelocity = 0;
    public float mass = 0;
    public bool useGravity = false;
}
