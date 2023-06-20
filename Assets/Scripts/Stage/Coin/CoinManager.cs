using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private TMP_Text _txtCounter;

    public void Initialize()
    {
        _spawnRadius = 5f;
        _txtCounter = GetComponentInChildren<TMP_Text>();
    }
    
    public void Spawn(int amount)
    {
        Vector3 pos = GameObject.Find("Enemy").transform.position;
        for (int i = 0; i < amount; i++)
        {
            float angle = i * Mathf.PI * 2 / amount;
            float x = Mathf.Cos(angle) * _spawnRadius;
            float z = Mathf.Sin(angle) * _spawnRadius;

            pos += new Vector3(x, 0, z);
            
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            
            Instantiate(_prefab, pos, rot);
        }
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
