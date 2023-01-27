using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{
    public Spell attackSpell;

    public override void Attack()
    {
        attackSpell.Cast(this, mobAIController.target.GetComponent<MobController>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletController bullet = other.GetComponent<BulletController>();
            mobAIController.lastAttackPosition = bullet.shooter.transform.position;
            TakeDamage(bullet.shooter, bullet.bulletDamage);
        }
    }
}
