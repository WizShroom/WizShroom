using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KnockBackEffect", menuName = "SpellEffects/KnockBackEffect", order = 0)]
public class KnockBackEffect : SpellEffect
{
    public float knockBackStrength = 0.1f;
    public string id;
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection = default(Vector3))
    {

        KnockBackDebuff knockBackDebuff = new KnockBackDebuff("Knockback" + id, 1f);
        if (mobHit.HasBuff(knockBackDebuff))
        {
            return;
        }
        mobHit.ApplyBuff(knockBackDebuff);

        mobHit.navMeshAgent.isStopped = true;
        Vector3 knockBackDirection = (mobHit.transform.position - bulletController.shooter.transform.position).normalized;
        if (hitDirection != default(Vector3))
        {
            knockBackDirection = hitDirection;
        }
        mobHit.navMeshAgent.velocity = knockBackDirection * knockBackStrength * bulletController.knockBackMultiplier;
        GameController.Instance.StartCoroutine(ResetStoppedAgent(mobHit, 0.3f));
    }
}