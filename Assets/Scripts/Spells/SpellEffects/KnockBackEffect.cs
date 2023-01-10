using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "KnockBackEffect", menuName = "SpellEffects/KnockBackEffect", order = 0)]
public class KnockBackEffect : SpellEffect
{
    public float knockBackStrength = 1;
    public string id;
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {

        KnockBackDebuff knockBackDebuff = new KnockBackDebuff("Knockback" + id, 1f);
        if (mobHit.HasBuff(knockBackDebuff))
        {
            return;
        }
        mobHit.ApplyBuff(knockBackDebuff);

        mobHit.navMeshAgent.isStopped = true;
        mobHit.navMeshAgent.velocity = hitDirection * knockBackStrength;
    }
}