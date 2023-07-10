using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    [SerializeField] private Enums.ActionType type;

    private GameObject _info;
    
    private Image _skillIcon;
    
    private TMP_Text _skillCost;
    private TMP_Text _skillCooldown;

    void Awake()
    {
        _info = transform.Find("Info").gameObject;
        
        _skillIcon = transform.Find("Icon").GetComponent<Image>();
        
        _skillCost = _info.transform.Find("Cost").GetComponent<TMP_Text>();
        _skillCooldown = transform.Find("Cooldown").GetComponent<TMP_Text>();
        
        _skillCooldown.gameObject.SetActive(false);
    }

    public void UpdateUI(WeaponItem weaponItem)
    {
        Tuple<bool, int> status = weaponItem.GetSkillStatus(type);
        bool onCooldown = status.Item1;
        int currentCooldown = status.Item2;
        
        ToggleCooldown(onCooldown);
        if (type == Enums.ActionType.Active)
        {
            if (onCooldown)
            {
                _skillCooldown.text = currentCooldown.ToString();
            }
            else
            {
                _skillIcon.sprite = weaponItem.primaryIcon;
                _skillCost.text   = weaponItem.GetSkillCost(Enums.ActionType.Active).ToString() + " SP";
                
                _skillIcon.enabled = true;
            }
        }
        else
        {
            if (onCooldown)
            {
                _skillCooldown.text = currentCooldown.ToString();
            }
            else
            {
                _skillIcon.sprite = weaponItem.secondaryIcon;
                _skillCost.text   = weaponItem.GetSkillCost(Enums.ActionType.Ultimate).ToString() + " SP";
                
                _skillIcon.enabled = true;
            }
        }
    }

    public void ToggleCooldown(bool isOnCooldown)
    {
        _info.gameObject.SetActive(!isOnCooldown);
        _skillIcon.gameObject.SetActive(!isOnCooldown);
        _skillCooldown.gameObject.SetActive(isOnCooldown);
    }
}
