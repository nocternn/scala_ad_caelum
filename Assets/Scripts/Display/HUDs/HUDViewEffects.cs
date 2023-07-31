using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDViewEffects : MonoBehaviour
{
    [SerializeField] private CardViewEffects _card01;
    [SerializeField] private CardViewEffects _card02;
    [SerializeField] private CardViewEffects _card03;

    public HashSet<int> usedCardsByPlayer;

    public EffectItem[] effects;
    public EffectItem selectedCard;

    public void Initialize()
    {
        usedCardsByPlayer = new HashSet<int>();
        selectedCard = null;
        
        _card01 = transform.Find("Cards").Find("Card01").GetComponent<CardViewEffects>();
        _card02 = transform.Find("Cards").Find("Card02").GetComponent<CardViewEffects>();
        _card03 = transform.Find("Cards").Find("Card03").GetComponent<CardViewEffects>();

        // Randomly retrieve 3 unused effects from the list of effects, with no repeats
        HashSet<int> usedCardsBySystem = new HashSet<int>();
        
        Tuple<EffectItem, int> card01 = GetRandomEffectItem(usedCardsBySystem);
        usedCardsBySystem.Add(card01.Item2);
        
        Tuple<EffectItem, int> card02 = GetRandomEffectItem(usedCardsBySystem);
        usedCardsBySystem.Add(card02.Item2);
        
        Tuple<EffectItem, int> card03 = GetRandomEffectItem(usedCardsBySystem);
        // ------

        _card01.UpdateUI(card01.Item1);
        _card02.UpdateUI(card02.Item1);
        _card03.UpdateUI(card03.Item1);
    }

    private Tuple<EffectItem, int> GetRandomEffectItem(HashSet<int> usedCardsID)
    {
        var usableCardsID = Enumerable.Range(0, effects.Length).Where(i =>
        {
            return !usedCardsByPlayer.Contains(i) && !usedCardsID.Contains(i);
        });
        
        var rand = new System.Random();
        int index = usableCardsID.ElementAt(rand.Next(0, usableCardsID.Count()));
        
        EffectItem card = effects[index];
        card.Initialize();
        
        return Tuple.Create(card, index);
    }

    private void SetSelectedCard()
    {
        if (_card01 == null || _card02 == null || _card03 == null)
            return;
            
        if (_card01.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card01.effect;
        }
        else if (_card02.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card02.effect;
        }
        else if (_card03.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card03.effect;
        }
        else
        {
            selectedCard = null;
        }
    }

    public void Confirm()
    {
        SetSelectedCard();
        HUDManager.Instance.Action(Enums.HUDActionType.SwitchCombat);
    }
}
