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
        if (collision.tag == "Player")
        {
            PlayerManager.Instance.isHit = true;
            Task.Delay((int)Mathf.Round(1000)).ContinueWith(t => { PlayerManager.Instance.isHit = false; });
            
            int enemyATK = EnemyManager.Instance.weaponSlotManager.GetDamage();
            int enemyCRT = EnemyManager.Instance.weaponSlotManager.GetCrit();

            int damage = 0;
            damage += EnemyManager.Instance.stats.GetOutgoingDamage(enemyATK, enemyCRT);
            damage += PlayerManager.Instance.stats.GetIncomingDamage(damage);

            Debug.Log("Player incoming damage = " + damage.ToString());
            PlayerManager.Instance.stats.TakeDamage(damage);
            
        }
        else if (collision.tag == "Enemy")
        {
            EnemyManager.Instance.isHit = true;
            Task.Delay((int)Mathf.Round(1000)).ContinueWith(t => { EnemyManager.Instance.isHit = false; });
            
            int playerATK = PlayerManager.Instance.weaponSlotManager.GetDamage();
            int playerCRT = PlayerManager.Instance.weaponSlotManager.GetCrit();

            int damage = 0;
            damage += PlayerManager.Instance.stats.GetOutgoingDamage(playerATK, playerCRT);
            damage += EnemyManager.Instance.stats.GetIncomingDamage(damage);

            Debug.Log("Enemy incoming damage = " + damage.ToString());
            
            EnemyManager.Instance.TakeDamage(damage);

            PlayerManager.Instance.stats.hitCount++;
            PlayerManager.Instance.stats.currentSkillPoints += 0.5f;
			if (PlayerManager.Instance.attacker.IsAttackOfType(Enums.ActionType.Basic))
            	PlayerManager.Instance.stats.UpdateAttackCharge(true);
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
