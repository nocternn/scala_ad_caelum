using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnRadius;

    public void Initialize()
    {
        _spawnRadius = 5f;
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
}
