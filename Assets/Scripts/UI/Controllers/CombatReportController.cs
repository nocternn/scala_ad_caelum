using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatReportController : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtRating;
    [SerializeField] private TMP_Text _txtTimer;
    [SerializeField] private TMP_Text _txtCoins;
    [SerializeField] private bool _initialized;

    public void Initialize()
    {
        if (_initialized)
            return;
        
        _txtRating = transform.GetChild(1).GetComponent<TMP_Text>();
        _txtTimer  = transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        _txtCoins  = transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>();

        _initialized = true;
    }

    public void SetRating(string rating)
    {
        _txtRating.text = rating;
    }

    public void SetTime(int elapsed)
    {
        _txtTimer.text = $"{elapsed / 60:00}:{elapsed % 60:00}";
        if (elapsed < 180) // 3 minutes * 60 seconds
        {
            SetRating("S");
        }
        else if (elapsed < 300)
        {
            SetRating("A");
        }
        else if (elapsed < 420)
        {
            SetRating("B");
        }
        else
        {
            SetRating("C");
        }
    }

    public void SetCoins(int coins)
    {
        _txtCoins.text = coins.ToString();
    }
}
