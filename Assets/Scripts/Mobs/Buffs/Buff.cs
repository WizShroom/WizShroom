using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public string name;
    public float duration;
    public bool canHaveMultiple = false;

    public Buff(string name, float duration, bool canHaveMultiple = false)
    {
        this.name = name;
        this.duration = duration;
        this.canHaveMultiple = canHaveMultiple;
    }

    public virtual void ApplyEffect()
    {

    }

    public virtual void RemoveEffect()
    {

    }
}