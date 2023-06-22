using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDBuffManager : MonoBehaviour
{
    [SerializeField] private HUDManager _manager;

    [SerializeField] private CardBuffController _card01;
    [SerializeField] private CardBuffController _card02;
    [SerializeField] private CardBuffController _card03;

    public HashSet<int> usedCardsByPlayer;

    public CardItem[] cards;
    public CardItem selectedCard;

    public Button btnConfirm;

    public void Initialize()
    {
        usedCardsByPlayer = new HashSet<int>();
        selectedCard = null;
        
        _card01 = transform.Find("Cards").Find("Card01").GetComponent<CardBuffController>();
        _card02 = transform.Find("Cards").Find("Card02").GetComponent<CardBuffController>();
        _card03 = transform.Find("Cards").Find("Card03").GetComponent<CardBuffController>();

        // Randomly retrieve 3 unused cards from the list of cards, with no repeats
        HashSet<int> usedCardsBySystem = new HashSet<int>();
        
        Tuple<CardItem, int> card01 = GetRandomCardItem(usedCardsBySystem);
        usedCardsBySystem.Add(card01.Item2);
        
        Tuple<CardItem, int> card02 = GetRandomCardItem(usedCardsBySystem);
        usedCardsBySystem.Add(card02.Item2);
        
        Tuple<CardItem, int> card03 = GetRandomCardItem(usedCardsBySystem);
        // ------

        _card01.UpdateUI(card01.Item1);
        _card02.UpdateUI(card02.Item1);
        _card03.UpdateUI(card03.Item1);

        btnConfirm = GetComponentInChildren<Button>();
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(delegate { _manager.Action(Enums.HUDAction.SwitchCombat); });
    }

    public void SetManager(HUDManager manager)
    {
        _manager = manager;
    }

    public Tuple<CardItem, int> GetRandomCardItem(HashSet<int> usedCardsID)
    {
        var usableCardsID = Enumerable.Range(0, cards.Length).Where(i =>
        {
            return !usedCardsByPlayer.Contains(i) && !usedCardsID.Contains(i);
        });
        
        var rand = new System.Random();
        int index = usableCardsID.ElementAt(rand.Next(0, usableCardsID.Count()));
        
        CardItem card = cards[index];
        card.Initialize();
        
        return Tuple.Create(card, index);
    }

    public void SetSelectedCard()
    {
        if (_card01 == null || _card02 == null || _card03 == null)
            return;
            
        if (_card01.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card01.card;
        }
        else if (_card02.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card02.card;
        }
        else if (_card03.transform.GetComponent<Toggle>().isOn)
        {
            selectedCard = _card03.card;
        }
        else
        {
            selectedCard = null;
        }
    }
}
