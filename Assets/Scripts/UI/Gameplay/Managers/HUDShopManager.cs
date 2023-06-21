using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class HUDShopManager : MonoBehaviour
{
    private StageManager _stage;
    
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    public void Initialize()
    {
        _container = transform.GetChild(1).GetChild(0).GetChild(0).transform;
        foreach (Transform child in _container.transform)
            Destroy(child.gameObject);
        
        foreach (CardItem buff in _stage.selectedBuffs)
        {
            CardBuffController card = Instantiate(_prefab, _container.transform).GetComponent<CardBuffController>();
            card.UpdateUI(buff);
        }
    }
    
    public void SetManager(StageManager stage)
    {
        _stage = stage;
    }

    public void Upgrade()
    {
        CardBuffController selectedCard = GetSelectedCard();
        if (selectedCard != null)
        {
            if (!selectedCard.card.canUpgrade)
                return;
            
            int coinsRequired = selectedCard.card.GetLevelUpCondition();
            if (_stage.coinsAvailable < coinsRequired)
                return;
            
            _stage.coinsAvailable -= coinsRequired;
            _stage.hud.hudStage.AddCoins(false, -coinsRequired);
            
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
