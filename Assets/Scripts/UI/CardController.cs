using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private Image _icon;
    private TMP_Text _title;
    private TMP_Text _description;

    public CardItem card;
    
    void Awake()
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _title = transform.Find("Title").GetComponent<TMP_Text>();
        _description = transform.Find("Description").GetComponent<TMP_Text>();
    }

    public void UpdateUI(CardItem cardItem)
    {
        _icon.sprite = cardItem.primaryIcon;
        _title.text = cardItem.name;
        _description.text = cardItem.description;

        card = cardItem;
    }
}
