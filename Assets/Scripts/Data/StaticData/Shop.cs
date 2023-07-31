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

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpenable = !isOpen;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isOpenable = false;
        }
    }
}
