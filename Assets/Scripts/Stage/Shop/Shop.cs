using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Interaction Configs")]
    public bool isOpen;
    public bool isOpenable;
    
    private void Awake()
    {
        isOpen = false;
        isOpenable = false;
    }
    
    public void Initialize()
    {
        isOpen = false;
        isOpenable = false;
    }
}
