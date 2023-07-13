using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    protected BoxCollider _damageCollider;

    private void Awake()
    {
        SetDamageCollider();
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (_damageCollider.tag.Equals(collision.tag))
            return;
        
        if (collision.tag == "Player")
        {
            // Toggle is hit state
            PlayerManager.Instance.isHit = true;
            Task.Delay((int)Mathf.Round(1000)).ContinueWith(t => { PlayerManager.Instance.isHit = false; });
            
            // Get enemy attack stats
            int enemyATK = EnemyManager.Instance.weaponSlotManager.GetDamage();
            int enemyCRT = EnemyManager.Instance.weaponSlotManager.GetCrit();

            // Calculate incoming damage
            int damage = 0;
            damage += EnemyManager.Instance.stats.GetOutgoingDamage(enemyATK, enemyCRT);
            damage += PlayerManager.Instance.stats.GetIncomingDamage(damage);

            // Register damage
            PlayerManager.Instance.stats.TakeDamage(damage);
            
            // Register total damage received stat
            StatisticsManager.Instance.playerStats.meta.totalDamageReceived += damage;
            StatisticsManager.Instance.WriteStatsPlayer();
        }
        else if (collision.tag == "Enemy")
        {
            // Toggle is hit state
            EnemyManager.Instance.isHit = true;
            Task.Delay((int)Mathf.Round(1000)).ContinueWith(t => { EnemyManager.Instance.isHit = false; });

            // Get player attack stats
            int playerATK = PlayerManager.Instance.weaponSlotManager.GetDamage();
            int playerCRT = PlayerManager.Instance.weaponSlotManager.GetCrit();

            // Calculate incoming damage
            int damage = 0;
            damage += PlayerManager.Instance.stats.GetOutgoingDamage(playerATK, playerCRT);
            damage += EnemyManager.Instance.stats.GetIncomingDamage(damage);

            // Register damage
            EnemyManager.Instance.TakeDamage(damage);

            // Register hit, increase SP, increase charge (if applicable)
            PlayerManager.Instance.stats.hitCount++;
            PlayerManager.Instance.stats.currentSkillPoints += 0.5f;
			if (PlayerManager.Instance.attacker.IsAttackOfType(Enums.ActionType.Basic))
            	PlayerManager.Instance.stats.UpdateAttackCharge(true);

            // Register total damage dealt stat
            StatisticsManager.Instance.playerStats.meta.totalDamageDealt += damage;
            // Register max single hit damage (if applicable)
            if (damage > StatisticsManager.Instance.playerStats.meta.maxDamageSingleHit)
                StatisticsManager.Instance.playerStats.meta.maxDamageSingleHit = damage;
            // Register new stats
            StatisticsManager.Instance.WriteStatsPlayer();
        }
    }
    protected virtual void OnTriggerExit(Collider collision)
    {
        // Empty
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
