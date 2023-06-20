using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private PlayerManager _target;

    void Start()
    {
        _speed = 10f;
        _target = GameObject.Find("Player").GetComponent<PlayerManager>();
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(
                transform.position,
                _target.lockOnTransform.position,
                _speed * Time.deltaTime
            );
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
