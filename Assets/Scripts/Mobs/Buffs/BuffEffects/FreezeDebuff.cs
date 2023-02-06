using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDebuff : Buff //TODO: make this a generic elemental debuff with enum to choose which one
{
    public MobController target;
    public MobController attacker;
    public IEnumerator coroutineReference;
    public DamageType damageType;

    public FreezeDebuff(string name, float duration, DamageType damageType, MobController target, MobController attacker = null) : base(name, duration)
    {
        this.target = target;
        this.attacker = attacker;
        this.damageType |= damageType;
    }

    public override void ApplyEffect()
    {
        target.navMeshAgent.speed -= target.navMeshAgent.speed;
    }

    public override void RemoveEffect()
    {
        //target.DecreaseResistanceValue(resistanceType, resistanceIncrease);
    }

}