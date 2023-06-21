using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    [SerializeField] private string type;
    
    private Image _skillIcon;
    private TMP_Text _skillCost;

    void Awake()
    {
        _skillIcon = transform.Find("Icon").GetComponent<Image>();
        _skillCost = transform.Find("Cost").GetComponent<TMP_Text>();
    }

    public void UpdateUI(WeaponItem weaponItem)
    {
        if (type.Equals("active"))
        {
            _skillIcon.sprite = weaponItem.primaryIcon;
            _skillCost.text   = weaponItem.active_cost.ToString() + " SP";
        }
        else
        {
            _skillIcon.sprite = weaponItem.secondaryIcon;
            _skillCost.text   = weaponItem.ultimate_cost.ToString() + " SP";
        }
        _skillIcon.enabled = true;
    }
}
