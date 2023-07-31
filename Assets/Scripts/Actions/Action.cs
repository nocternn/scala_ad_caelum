using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    #region Attributes
    
    [Header("Common Properties")]
    public int id;
    [SerializeField] protected string _animation;
    [SerializeField] protected float _duration;
    [SerializeField] protected float _recoveryTime;

    #endregion

    protected virtual void Awake()
    {
        // Empty
    }
    protected virtual void Update()
    {
        // Empty
    }

    public virtual void Initialize()
    {
        
    }
    
    public virtual void PerformAction(PlayerManager player, bool playAnimation = true)
    {
        // Empty
    }
    
    public virtual void PerformAction(EnemyManager enemy, bool playAnimation = true)
    {
        enemy.stats.currentRecoveryTime = _recoveryTime;
    }
}
