using UnityEngine;

[CreateAssetMenu(fileName = "HealthBuff", menuName = "SpellEffects/HealthBuff", order = 0)]
public class HealthBuff : SpellEffect
{
    public override void OnMobEffect(MobController affectedMob, Vector3 castDirection = default)
    {
        StatBuff healthBuff = new StatBuff("HealthBuff", 10, 10, StatType.Health, affectedMob);
        affectedMob.ApplyBuff(healthBuff);
    }
}