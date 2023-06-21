using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuManager : MonoBehaviour
{
    [SerializeField] private MenuManager _manager;
    
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    public WeaponItem[] weapons;

    void Awake()
    {
        _container = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);
        
        foreach (WeaponItem weapon in weapons)
        {
            CardWeaponController card = Instantiate(_prefab, _container.transform).GetComponent<CardWeaponController>();
            card.UpdateUI(weapon);
            card.transform.GetComponent<Toggle>().group = _container.transform.GetComponent<ToggleGroup>();
        }
    }

    public void SetManager(MenuManager manager)
    {
        _manager = manager;
    }

    public void Back()
    {
        _manager.menuType = Enums.MenuType.Main;

        _manager.inventoryMenu.gameObject.SetActive(false);
        _manager.mainMenu.gameObject.SetActive(true);
    }

    public void Choose()
    {
        foreach (Transform child in _container)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                CardWeaponController selectedCard = child.GetComponent<CardWeaponController>();
                selectedCard.transform.GetComponent<Toggle>().isOn = false;

                _manager.sceneLoader.weapon = selectedCard.weapon;
                
                break;
            }
        }

        Back();
    }
}
