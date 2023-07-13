using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("Properties")]
    protected Image _icon;
    protected TMP_Text _title;
    protected TMP_Text _description;
    
    public virtual void Awake()
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _title = transform.Find("Title").GetComponent<TMP_Text>();
        _description = transform.Find("Description").GetComponent<TMP_Text>();
    }
}
