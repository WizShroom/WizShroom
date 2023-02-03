using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceBuff : Buff
{
    public int resistanceIncrease;
    public ResistanceType resistanceType;
    public MobController target;

    public ResistanceBuff(string name, float duration, int resistanceIncrease, ResistanceType resistanceType, MobController target, bool canHaveMultiple = false) : base(name, duration, canHaveMultiple)
    {
        this.resistanceIncrease = resistanceIncrease;
        this.target = target;
        this.resistanceType = resistanceType;
    }

    public override void ApplyEffect()
    {
        target.IncreaseResistanceValue(resistanceType, resistanceIncrease);
    }

    public override void RemoveEffect()
    {
        target.DecreaseResistanceValue(resistanceType, resistanceIncrease);
    }

}