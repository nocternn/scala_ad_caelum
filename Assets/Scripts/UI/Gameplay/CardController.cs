using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("Properties")]
    private Image _icon;
    private TMP_Text _title;
    private TMP_Text _description;
    private TMP_Text _cost;

    [Header("Items")]
    public CardItem card;
    public WeaponItem weapon;
    
    [Header("Properties")]
    [SerializeField] private bool _isShop;
    
    void Awake()
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _title = transform.Find("Title").GetComponent<TMP_Text>();
        _description = transform.Find("Description").GetComponent<TMP_Text>();
        if (_isShop)
            _cost = transform.Find("Cost").GetComponent<TMP_Text>();
    }

    public void UpdateUI(CardItem cardItem)
    {
        _icon.sprite = cardItem.primaryIcon;
        _title.text = cardItem.GetName();
        _description.text = cardItem.GetDescription();
        if (_isShop)
            _cost.text = cardItem.GetCost();

        card = cardItem;
    }
    
    public void UpdateUI(WeaponItem weaponItem)
    {
        _icon.sprite = weaponItem.primaryIcon;
        _title.text = weaponItem.name;
        _description.text = weaponItem.description;

        weapon = weaponItem;
    }
}
