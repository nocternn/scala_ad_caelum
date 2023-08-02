using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDamageCollider : DamageCollider
{
    public Transform item;

    private float maxRange = 25.0f;

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        
        if (_damageCollider.tag.Equals(collision.tag))
            return;

        DestroyAmmo();
    }

    void Update()
    {
        if (Mathf.Abs(item.position.x) > maxRange || item.position.y < 0 || Mathf.Abs(item.position.z) > maxRange)
            DestroyAmmo();
    }
    
    protected void DestroyAmmo()
    {
        PlayerManager.Instance.weaponSlotManager.DisableDamageCollider();
        EnemyManager.Instance.weaponSlotManager.DisableDamageCollider();
        Destroy(item.gameObject);
    }
}
