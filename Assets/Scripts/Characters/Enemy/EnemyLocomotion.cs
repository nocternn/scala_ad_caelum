using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : CharacterLocomotion
{
    private EnemyManager _manager;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }


}
