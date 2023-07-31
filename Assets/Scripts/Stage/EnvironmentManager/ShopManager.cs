using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    
    private void Awake()
    {
        _shop = GameObject.FindObjectsOfType<Shop>(true)[0];
    }
    
    public void Initialize()
    {
        ToggleVisibility(false);
    }
    
    public void ToggleVisibility(bool visible)
    {
        _shop.gameObject.SetActive(visible);
        if (!visible)
            _shop.isOpenable = false;
    }

    public bool IsOpen()
    {
        return _shop.isOpen;
    }
    public bool IsOpenable()
    {
        return _shop.isOpenable;
    }
    
    public void Open()
    {
        _shop.isOpen = true;
    }
    
    public void Close()
    {
        _shop.isOpen = false;
    }
}
