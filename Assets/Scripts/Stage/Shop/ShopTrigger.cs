using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private Shop _shop;

    void Awake()
    {
        _shop = GameObject.FindObjectsOfType<Shop>(true)[0];
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!_shop.isOpen)
            {
                _shop.isOpenable = true;
            }
            else
            {
                _shop.isOpenable = false;
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _shop.isOpenable = false;
        }
    }
}
