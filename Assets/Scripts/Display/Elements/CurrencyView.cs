using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtCounter;
    
    public void Initialize()
    {
        _txtCounter = GetComponentInChildren<TMP_Text>();
    }
    
    public void AddCoins(int amount)
    {
        _txtCounter.text = $"{GetCoins() + amount}";
    }
    
    public void SetCoins(int amount)
    {
        _txtCounter.text = $"{amount}";
    }

    public int GetCoins()
    {
        int currentAmount;
        _ = int.TryParse(_txtCounter.text, out currentAmount);
        
        return currentAmount;
    }
}
