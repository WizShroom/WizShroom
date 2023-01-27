using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KnockBackEffect", menuName = "SpellEffects/KnockBackEffect", order = 0)]
public class KnockBackEffect : SpellEffect
{
    public float knockBackStrength = 0.1f;
    public string id;
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection = default(Vector3))
    {

        ApplyKnockback(bulletController.shooter, mobHit, bulletController.knockBackMultiplier, hitDirection);

    }

    public override void OnMobEffect(MobController casterMob, MobController affectedMob, Vector3 hitDirection = default(Vector3))
    {
        Collider[] colliders = Physics.OverlapBox(casterMob.shootPoint.position + Vector3.right * 0.3f, new Vector3(0.5f, 0.5f, 0.5f), casterMob.shootPoint.rotation, LayerMask.GetMask("Player", "Enemy", "NPC"));
        foreach (Collider collider in colliders)
        {
            if (casterMob.gameObject.layer != collider.gameObject.layer)
            {
                MobController hitMob = collider.GetComponent<MobController>();
                ApplyKnockback(casterMob, hitMob, 30, hitDirection);
            }
        }
    }

    public void ApplyKnockback(MobController casterMob, MobController mobHit, float knockBackMultiplier, Vector3 hitDirection = default(Vector3))
    {
        KnockBackDebuff knockBackDebuff = new KnockBackDebuff("Knockback" + id, 1f);
        if (mobHit.HasBuff(knockBackDebuff))
        {
            return;
        }
        mobHit.ApplyBuff(knockBackDebuff);

        Vector3 knockBackDirection = (mobHit.transform.position - casterMob.transform.position).normalized;
        if (hitDirection != default(Vector3))
        {
            knockBackDirection = hitDirection;
        }
        Vector3 knockbackToApply = knockBackDirection * knockBackStrength * knockBackMultiplier;
        mobHit.navMeshAgent.Warp(mobHit.transform.position + knockbackToApply);
    }

}