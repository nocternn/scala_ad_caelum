using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected HUDManager _hudManager;
    
    [Header("Base Stats")]
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public int baseCritical;
    
    [Header("Current Stats")]
    public int currentHealth;
    public int currentAttack;
    public int currentDefense;
    public int currentCritical;
    
    [Header("Max Stats")]
    public int maxHealth;
    public int maxAttack;
    public int maxDefense;
    public int maxCritical;
    
    [Header("Stats Scale")]
    public int scaleHealth = 1;
    public int scaleAttack = 1;
    public int scaleDefense = 1;
    public int scaleCritical = 1;
    
    public void SetManager(HUDManager manager)
    {
        _hudManager = manager;
    }
}
