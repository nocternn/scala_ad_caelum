using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuViewInventory : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    public WeaponItem[] weapons;

    private void Awake()
    {
        _container = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);
        
        foreach (WeaponItem weapon in weapons)
        {
            CardViewWeapon card = Instantiate(_prefab, _container.transform).GetComponent<CardViewWeapon>();
            card.UpdateUI(weapon);
            card.transform.GetComponent<Toggle>().group = _container.transform.GetComponent<ToggleGroup>();
        }
    }

    public void Choose()
    {
        foreach (Transform child in _container)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                CardViewWeapon selectedCard = child.GetComponent<CardViewWeapon>();
                selectedCard.transform.GetComponent<Toggle>().isOn = false;

                // Register player weapon
                StatisticsManager.Instance.playerWeapon = selectedCard.weapon;
                StatisticsManager.Instance.playerStats.weapon = selectedCard.weapon.name;
                StatisticsManager.Instance.WriteStatsPlayer();
                
                break;
            }
        }

        MenuManager.Instance.Back();
    }

    public WeaponItem GetWeapon(string name)
    {
        foreach (var weapon in weapons)
        {
            if (weapon.name.Equals(name))
                return weapon;
        }
        return null;
    }
}
