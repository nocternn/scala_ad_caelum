using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardBuffController : CardController
{
    public CardItem card;
    
    private TMP_Text _cost;
    [SerializeField] private bool _isShop;

    public override void Awake()
    {
        base.Awake();
        
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
}
