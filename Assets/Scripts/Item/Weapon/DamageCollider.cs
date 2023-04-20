using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private BoxCollider _damageCollider;
    private PlayerManager _playerManager;
    
    public int currentWeaponDamage = 100;

    private void Awake()
    {
        _damageCollider = GetComponent<BoxCollider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;

        _playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            _playerManager.TakeDamage(currentWeaponDamage);
        }
        else if (collision.tag == "Enemy")
        {
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if (enemy == null)
                return;
            enemy.TakeDamage(currentWeaponDamage);

			if (_playerManager.IsBasicAttack())
            	_playerManager.UpdateAttackCharge(true);
        }
    }

    public void Enable()
    {
        _damageCollider.enabled = true;
    }
    
    public void Disable()
    {
        _damageCollider.enabled = false;
    }
}
