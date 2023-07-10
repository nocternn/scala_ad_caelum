using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDamageCollider : DamageCollider
{
    public AmmoItem ammoItem;

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
        Vector3 pos = transform.root.position;
        if (pos.x > maxRange || pos.y > maxRange || pos.z > maxRange)
        {
            DestroyAmmo();
        }
    }
    
    protected void DestroyAmmo()
    {
        PlayerManager.Instance.weaponSlotManager.DisableDamageCollider();
        EnemyManager.Instance.weaponSlotManager.DisableDamageCollider();
        Destroy(transform.root.gameObject);
    }
}
