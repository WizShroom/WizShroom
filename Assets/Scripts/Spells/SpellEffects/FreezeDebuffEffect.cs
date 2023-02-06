using UnityEngine;

[CreateAssetMenu(fileName = "FreezeDebuffEffect", menuName = "SpellEffects/FreezeDebuffEffect", order = 0)]
public class FreezeDebuffEffect : SpellEffect
{
    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {
        FreezeDebuff freezeDebuff = new FreezeDebuff("FreezeEffect", 5, DamageType.BURN, mobHit, bulletController.shooter);
        mobHit.ApplyBuff(freezeDebuff);
    }
}