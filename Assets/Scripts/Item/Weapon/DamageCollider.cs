using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected BoxCollider _damageCollider;
    protected PlayerManager _player;
    protected EnemyManager _enemy;

    private void Awake()
    {
        SetDamageCollider();

        _player = GameObject.FindObjectsOfType<PlayerManager>(true)[0];
        _enemy = GameObject.FindObjectsOfType<EnemyManager>(true)[0];
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            int enemyATK = _enemy.weaponSlotManager.GetDamage();
            int enemyCRT = _enemy.weaponSlotManager.GetCrit();

            int damage = 0;
            damage += _enemy.stats.GetOutgoingDamage(enemyATK, enemyCRT);
            damage += _player.stats.GetIncomingDamage(damage);

            Debug.Log("Player incoming damage = " + damage.ToString());
            
            _player.stats.TakeDamage(damage);
        }
        else if (collision.tag == "Enemy")
        {
            EnemyManager enemyManager = collision.GetComponent<EnemyManager>();
            if (enemyManager == null)
                return;
            
            int playerATK = _player.weaponSlotManager.GetDamage();
            int playerCRT = _player.weaponSlotManager.GetCrit();

            int damage = 0;
            damage += _player.stats.GetOutgoingDamage(playerATK, playerCRT);
            damage += enemyManager.stats.GetIncomingDamage(damage);

            Debug.Log("Enemy incoming damage = " + damage.ToString());
            
            enemyManager.TakeDamage(damage);

            _player.stats.hitCount++;
            _player.stats.currentSkillPoints += 0.5f;
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
