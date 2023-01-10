using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public float health;
    public int maxHealth;

    public float experience;
    public float experienceOnKill;
    public int experienceForLevelUp;
    public int currentLevel;

    public bool canDie = true;

    public Transform castPosition;

    public List<Stat> stats = new List<Stat>(){
        new Stat(1, StatType.Health),
        new Stat(1, StatType.Strength),
        new Stat(1, StatType.Agility),
        new Stat(1, StatType.Intelligence),
        new Stat(1, StatType.Wisdom),
        new Stat(1, StatType.Luck),
    };

    public List<Buff> buffs = new List<Buff>();

    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        UpdateMobValues();
    }

    public int GetStatValueByType(StatType statType)
    {
        foreach (Stat stat in stats)
        {
            if (stat.GetStatType() == statType)
            {
                return stat.GetValue();
            }
        }
        return 0;
    }

    public Stat GetStatByType(StatType statType)
    {
        foreach (Stat stat in stats)
        {
            if (stat.GetStatType() == statType)
            {
                return stat;
            }
        }
        return null;
    }

    public void IncreaseStatValue(StatType statType, int increaseAmount = 1)
    {
        Stat statToImprove = GetStatByType(statType);
        statToImprove.IncreaseValue(increaseAmount);
        UpdateMobValues();
    }

    public void DecreaseStatValue(StatType statType, int decreaseAmount = 1)
    {
        Stat statToDecrease = GetStatByType(statType);
        statToDecrease.DecreaseValue(decreaseAmount);
        UpdateMobValues();
    }

    private void UpdateMobValues()
    {
        health = maxHealth = GetStatValueByType(StatType.Health) * 10;
    }

    public virtual void TakeDamage(MobController damager, float incomingDamage = 1, DamageType damageType = DamageType.BRUTE)
    {
        float finalDamage = 0;

        switch (damageType)
        {
            case DamageType.BRUTE:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Health) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Strength) / (600f + GetStatValueByType(StatType.Strength))));
                break;

            case DamageType.BURN:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Strength) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Strength) / (600f + GetStatValueByType(StatType.Strength))));
                break;

            case DamageType.NATURE:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Agility) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Strength) / (600f + GetStatValueByType(StatType.Strength))));
                break;

            case DamageType.MAGIC:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Intelligence) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Wisdom) / (600f + GetStatValueByType(StatType.Wisdom))));
                break;

            case DamageType.DARK:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Wisdom) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Intelligence) / (600f + GetStatValueByType(StatType.Intelligence))));
                break;

            case DamageType.KARMA:
                finalDamage = incomingDamage *
                    (1 - (GetStatValueByType(StatType.Luck) / 500f)) *
                    (1 - (GetStatValueByType(StatType.Luck) / (2000f + Random.Range(-500f, 500f)))) *
                    (1 - (GetStatValueByType(StatType.Wisdom) / (600f + GetStatValueByType(StatType.Wisdom))));
                break;
        }

        finalDamage = Mathf.Round(finalDamage * 100f) / 100f;

        health -= finalDamage;
        if (health <= 0 && canDie)
        {
            Death(damager);
        }
    }

    public virtual void Death(MobController damager)
    {
        if (damager)
        {
            damager.IncreaseExperience(experienceOnKill);
        }
        Destroy(gameObject);
    }

    public virtual void IncreaseExperience(float increaseAmount)
    {
        experience += increaseAmount;
        if (experience >= experienceForLevelUp)
        {
            LevelUp();
        }
    }

    public virtual void LevelUp()
    {
        currentLevel++;
        experienceForLevelUp *= 2;
    }

    public virtual void ApplyBuff(Buff buffToApply)
    {
        if (HasBuff(buffToApply) && !buffToApply.canHaveMultiple)
        {
            return;
        }

        buffs.Add(buffToApply);
        buffToApply.ApplyEffect();
        if (buffToApply.duration <= 0)
        {
            return;
        }
        StartCoroutine(RemoveBuffAfterDuration(buffToApply));
    }

    public virtual void RemoveBuff(Buff buffToRemove)
    {
        buffToRemove.RemoveEffect();
        buffs.Remove(buffToRemove);
    }

    public virtual IEnumerator RemoveBuffAfterDuration(Buff buffToRemove)
    {
        yield return new WaitForSeconds(buffToRemove.duration);
        RemoveBuff(buffToRemove);
    }

    public virtual bool HasBuff(Buff buffToCheck)
    {
        Buff existingBuff = buffs.Find(buff => buff.name == buffToCheck.name);
        if (existingBuff != null)
        {
            return true;
        }
        return false;
    }
}