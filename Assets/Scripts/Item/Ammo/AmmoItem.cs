using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Ammo")]
public class AmmoItem : Item
{
    [Header("Specific Item Information")]
    public Enums.AmmoType type;
    public GameObject model;

    [Header("Physics")]
    public float forwardVelocity = 550;
    public float upwardVelocity = 0;
    public float mass = 0;
    public bool useGravity = false;
}
