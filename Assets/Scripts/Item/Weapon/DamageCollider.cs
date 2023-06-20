using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected BoxCollider _damageCollider;
    protected PlayerManager _player;
    protected EnemyManager _enemy;
    
    public int currentWeaponDamage = 100;

    private void Awake()
    {
        SetDamageCollider();

        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _enemy = GameObject.Find("Enemy").GetComponent<EnemyManager>();
    }

    protected virtual void OnTriggerEnter(Collider collision)
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

    protected void SetDamageCollider()
    {
        _damageCollider = GetComponent<BoxCollider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;
    }
    
    protected void DestroyAmmo()
    {
        _player.weaponSlotManager.DisableDamageCollider();
        _enemy.weaponSlotManager.DisableDamageCollider();
        Destroy(transform.root.gameObject);
    }
    
    public void Enable()
    {
        if (_damageCollider == null)
            SetDamageCollider();
        _damageCollider.enabled = true;
    }
    
    public void Disable()
    {
        if (_damageCollider == null)
            return;
        _damageCollider.enabled = false;
    }
}
