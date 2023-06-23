using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class HUDShopManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    public void Initialize()
    {
        _container = transform.GetChild(1).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);
        
        foreach (CardItem buff in StageManager.Instance.selectedBuffs)
        {
            CardBuffController card = Instantiate(_prefab, _container.transform).GetComponent<CardBuffController>();
            card.UpdateUI(buff);
            card.transform.GetComponent<Toggle>().group = _container.transform.GetComponent<ToggleGroup>();
        }
    }

    public void Upgrade()
    {
        CardBuffController selectedCard = GetSelectedCard();
        if (selectedCard != null)
        {
            if (!selectedCard.card.canUpgrade)
                return;
            
            int coinsRequired = selectedCard.card.GetLevelUpCondition();
            if (StageManager.Instance.coinsAvailable < coinsRequired)
                return;
            
            StageManager.Instance.coinsAvailable -= coinsRequired;
            HUDManager.Instance.hudStage.AddCoins(false, -coinsRequired);
            
            selectedCard.card.LevelUp();
            selectedCard.UpdateUI(selectedCard.card);

            selectedCard.transform.GetComponent<Toggle>().isOn = false;
        }
    }
    
    private CardBuffController GetSelectedCard()
    {
        foreach (Transform child in _container)
        {
            if (child.GetComponent<Toggle>().isOn)
            {
                return child.GetComponent<CardBuffController>();
            }
        }
        return null;
    }
}
