using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    [SerializeField] private string type;

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
        if (type.Equals("active"))
        {
            ToggleCooldown(weaponItem.skillActive.onCooldown);
            if (weaponItem.skillActive.onCooldown)
            {
                _skillCooldown.text = weaponItem.skillActive.currentCooldown.ToString();
            }
            else
            {
                _skillIcon.sprite = weaponItem.primaryIcon;
                _skillCost.text   = weaponItem.activeCost.ToString() + " SP";
                
                _skillIcon.enabled = true;
            }
        }
        else
        {
            ToggleCooldown(weaponItem.skillUltimate.onCooldown);
            if (weaponItem.skillUltimate.onCooldown)
            {
                _skillCooldown.text = weaponItem.skillUltimate.currentCooldown.ToString();
            }
            else
            {
                _skillIcon.sprite = weaponItem.secondaryIcon;
                _skillCost.text   = weaponItem.ultimateCost.ToString() + " SP";
                
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
