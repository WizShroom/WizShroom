using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Buff
{
    public float damagePerSecond;
    public MobController target;
    public MobController attacker;
    public IEnumerator coroutineReference;
    public DamageType damageType;

    public FireDebuff(string name, float duration, float damagePerSecond, DamageType damageType, MobController target, MobController attacker = null) : base(name, duration)
    {
        this.damagePerSecond = damagePerSecond;
        this.target = target;
        this.attacker = attacker;
        this.damageType |= damageType;
    }

    public override void ApplyEffect()
    {
        coroutineReference = ApplyDamageOvertime();
        target.StartCoroutine(coroutineReference);
    }

    public override void RemoveEffect()
    {
        target.StopCoroutine(coroutineReference);
    }

    public IEnumerator ApplyDamageOvertime()
    {
        while (true)
        {
            target.TakeDamage(attacker, damagePerSecond, damageType);
            yield return new WaitForSeconds(1f);
        }
    }

}