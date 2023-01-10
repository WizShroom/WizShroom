using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{

    public MobAIController mobAIController;

    private void Update()
    {
        if (mobAIController)
        {
            mobAIController.CallUpdateState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            BulletController bullet = other.GetComponent<BulletController>();
            mobAIController.lastAttackPosition = bullet.shooter.transform.position;
            TakeDamage(bullet.shooter, bullet.bulletDamage);
        }
    }
}
