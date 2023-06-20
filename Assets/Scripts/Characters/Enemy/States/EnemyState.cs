using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyManager _manager;
    
    public abstract EnemyState Tick(EnemyManager manager);
    
    public void SetManager(EnemyManager manager)
    {
        _manager = manager;
    }
}
