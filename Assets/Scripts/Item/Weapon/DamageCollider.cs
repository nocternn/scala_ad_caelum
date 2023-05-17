using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private BoxCollider _damageCollider;
    private PlayerManager _player;
    private EnemyManager _enemy;
    
    public int currentWeaponDamage = 100;

    private void Awake()
    {
        _damageCollider = GetComponent<BoxCollider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;

        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _enemy = GameObject.Find("Enemy").GetComponent<EnemyManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            int damage = _enemy.stats.GetOutgoingDamage(currentWeaponDamage);
            damage = _player.stats.GetIncomingDamage(damage);

            Debug.Log("Player incoming damage = " + damage.ToString());
            
            _player.stats.TakeDamage(damage);
        }
        else if (collision.tag == "Enemy")
        {
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();
            if (enemyManager == null)
                return;

            int damage = _player.stats.GetOutgoingDamage(currentWeaponDamage);
            damage = enemyManager.stats.GetIncomingDamage(damage);

            Debug.Log("Enemy incoming damage = " + damage.ToString());
            
            enemyManager.TakeDamage(damage);

            _player.stats.hitCount++;
			if (_player.attacker.IsBasicAttack())
            	_player.stats.UpdateAttackCharge(true);
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
