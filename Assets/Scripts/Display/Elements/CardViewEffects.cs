using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardViewEffects : CardView
{
    public EffectItem effect;
    
    private TMP_Text _cost;
    [SerializeField] private bool _isShop;

    public override void Awake()
    {
        base.Awake();
        
        if (_isShop)
            _cost = transform.Find("Cost").GetComponent<TMP_Text>();
    }
    
    public void UpdateUI(EffectItem effectItem)
    {
        _icon.sprite = effectItem.primaryIcon;
        _title.text = effectItem.GetName();
        _description.text = effectItem.GetDescription();

        if (_isShop)
            _cost.text = effectItem.GetCost();

        effect = effectItem;
    }
}
