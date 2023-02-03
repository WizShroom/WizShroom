using UnityEngine;

[CreateAssetMenu(fileName = "FireDebuffEffect", menuName = "SpellEffects/FireDebuffEffect", order = 0)]
public class FireDebuffEffect : SpellEffect
{
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {
        FireDebuff fireDebuff = new FireDebuff("FireDamage", 3, 1, DamageType.BURN, mobHit, bulletController.shooter);
        mobHit.ApplyBuff(fireDebuff);
    }
}