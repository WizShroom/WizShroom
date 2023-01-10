using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBuff : Buff
{
    public int statIncrease;
    public StatType statType;
    public MobController target;

    public StatBuff(string name, float duration, int statIncrease, StatType statType, MobController target) : base(name, duration)
    {
        this.statIncrease = statIncrease;
        this.target = target;
        this.statType = statType;
    }

    public override void ApplyEffect()
    {
        target.IncreaseStatValue(statType, statIncrease);
    }

    public override void RemoveEffect()
    {
        target.DecreaseStatValue(statType, statIncrease);
    }

}