using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public string statName = "";
    public int statValue = 0;
    public StatType statType;

    public Stat(int statValue, StatType statType)
    {
        this.statValue = statValue;
        this.statType = statType;
        this.statName = statType.ToString();
    }

    public void IncreaseValue(int increaseAmount = 1)
    {
        statValue += increaseAmount;
    }

    public void DecreaseValue(int decreaseAmount = 1)
    {
        statValue -= decreaseAmount;
    }

    public void SetValue(int value)
    {
        statValue = value;
    }

    public int GetValue()
    {
        return statValue;
    }

    public StatType GetStatType()
    {
        return statType;
    }
}

public enum StatType
{
    Health,
    Strength,
    Agility,
    Intelligence,
    Wisdom,
    Luck
}

[System.Flags]
public enum DamageType
{
    BRUTE = 1 << 0,
    BURN = 1 << 1,
    NATURE = 1 << 2,
    MAGIC = 1 << 3,
    DARK = 1 << 4,
    KARMA = 1 << 5,
}