using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack")]
public class EnemyActionAttack : EnemyAction
{
    public int score = 3;
    public float recoveryTime = 1;

    public float minAngle = -50;
    public float maxAngle = 50;

    public float minDistance = 0;
    public float maxDistance = 5;
}
