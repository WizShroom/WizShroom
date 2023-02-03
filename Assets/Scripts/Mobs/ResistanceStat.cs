using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResistanceStat
{
    public string resistanceName = "";
    public int resistanceValue = 0;
    public ResistanceType resistanceType;

    public ResistanceStat(int resistanceValue, ResistanceType resistanceType)
    {
        this.resistanceValue = resistanceValue;
        this.resistanceType = resistanceType;
        this.resistanceName = resistanceType.ToString();
    }

    public void IncreaseValue(int increaseAmount = 1)
    {
        resistanceValue += increaseAmount;
    }

    public void DecreaseValue(int decreaseAmount = 1)
    {
        resistanceValue -= decreaseAmount;
    }

    public void SetValue(int value)
    {
        resistanceValue = value;
    }

    public int GetValue()
    {
        return resistanceValue;
    }

    public ResistanceType GetResistanceType()
    {
        return resistanceType;
    }
}

public enum ResistanceType
{
    BRUTE,
    BURN,
    NATURE,
    MAGIC,
    DARK,
    KARMA,
}