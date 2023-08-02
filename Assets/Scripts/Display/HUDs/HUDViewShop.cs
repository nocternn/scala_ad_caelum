using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class HUDViewShop : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    public void Initialize()
    {
        Cursor.lockState = CursorLockMode.Confined;
        
        _container = transform.GetChild(2).GetChild(0).GetChild(0);
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);
        
        foreach (EffectItem buff in StageManager.Instance.selectedEffects)
        {
            CardViewEffects card = Instantiate(_prefab, _container.transform).GetComponent<CardViewEffects>();
            card.UpdateUI(buff);
            card.transform.GetComponent<Toggle>().group = _container.transform.GetComponent<ToggleGroup>();
        }
    }

    public void Upgrade()
    {
        CardViewEffects selectedCard = GetSelectedCard();
        if (selectedCard != null)
        {
            if (!selectedCard.effect.canUpgrade)
                return;
            
            int coinsRequired = selectedCard.effect.GetLevelUpCondition();
            if (StageManager.Instance.coinsAvailable < coinsRequired)
                return;
            
            StageManager.Instance.coinsAvailable -= coinsRequired;
            HUDManager.Instance.hudStage.AddCoins(false, -coinsRequired);
            
            selectedCard.effect.LevelUp();
            selectedCard.UpdateUI(selectedCard.effect);

            selectedCard.transform.GetComponent<Toggle>().isOn = false;
        }
    }
    
    private CardViewEffects GetSelectedCard()
    {
        foreach (Transform child in _container)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                return child.GetComponent<CardViewEffects>();
            }
        }
        return null;
    }
}
