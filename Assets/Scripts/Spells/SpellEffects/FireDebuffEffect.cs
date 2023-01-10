using UnityEngine;

[CreateAssetMenu(fileName = "FireDebuffEffect", menuName = "SpellEffects/FireDebuffEffect", order = 0)]
public class FireDebuffEffect : SpellEffect
{
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController)
    {
        FireDebuff fireDebuff = new FireDebuff("FireDamage", 3, 1, DamageType.BURN, bulletController.target, bulletController.shooter);
        bulletController.target.ApplyBuff(fireDebuff);
    }
}