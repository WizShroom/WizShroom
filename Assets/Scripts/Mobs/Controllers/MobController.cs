using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MobController : MonoBehaviour
{
    public float health;
    public int maxHealth;

    public float mana;
    public int maxMana;

    public float experience;
    public float experienceOnKill;
    public int experienceForLevelUp;
    public int currentLevel;

    public bool canDie = true;

    public List<Stat> stats = new List<Stat>(){
        new Stat(1, StatType.Health),
        new Stat(1, StatType.Strength),
        new Stat(1, StatType.Agility),
        new Stat(1, StatType.Intelligence),
        new Stat(1, StatType.Wisdom),
        new Stat(1, StatType.Luck),
    };

    public List<ResistanceStat> resistances = new List<ResistanceStat>()
    {
        new ResistanceStat(0, ResistanceType.BRUTE),
        new ResistanceStat(0, ResistanceType.BURN),
        new ResistanceStat(0, ResistanceType.NATURE),
        new ResistanceStat(0, ResistanceType.MAGIC),
        new ResistanceStat(0, ResistanceType.DARK),
        new ResistanceStat(0, ResistanceType.KARMA),
    };

    public List<Buff> buffs = new List<Buff>();

    public AudioSource audioSource;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public MobSpawner spawner;
    [HideInInspector] public MobAIController mobAIController;
    [HideInInspector] public bool paused = false;

    [HideInInspector] public bool disabled = false;
    [HideInInspector] public float elapsedAttackTime;

    public Transform pivotPoint;
    public Transform shootPoint;

    public string mobID;

    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
    }

    private void OnDestroy()
    {
        OnObjDestroy();
    }

    public virtual void OnObjDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    private void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        MobAIController mobAIController;
        TryGetComponent<MobAIController>(out mobAIController);
        if (mobAIController)
        {
            this.mobAIController = mobAIController;
        }
        UpdateMobValues();
    }

    private void Update()
    {
        if (disabled)
        {
            return;
        }
        if (mobAIController && !paused)
        {
            mobAIController.CallUpdateState();
        }
        OnUpdate();
    }

    public virtual void OnUpdate() { }

    public virtual void Attack() { }

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

    public int GetResistanceValueByType(ResistanceType resistanceType)
    {
        foreach (ResistanceStat resistance in resistances)
        {
            if (resistance.GetResistanceType() == resistanceType)
            {
                return resistance.GetValue();
            }
        }
        return 0;
    }

    public ResistanceStat GetResistanceByType(ResistanceType resistanceType)
    {
        foreach (ResistanceStat resistance in resistances)
        {
            if (resistance.GetResistanceType() == resistanceType)
            {
                return resistance;
            }
        }
        return null;
    }

    public void IncreaseResistanceValue(ResistanceType resistanceType, int increaseAmount = 1)
    {
        ResistanceStat resistanceToImprove = GetResistanceByType(resistanceType);
        resistanceToImprove.IncreaseValue(increaseAmount);
        UpdateMobValues();
    }

    public void DecreaseResistanceValue(ResistanceType resistanceType, int decreaseAmount = 1)
    {
        ResistanceStat resistanceToDecrease = GetResistanceByType(resistanceType);
        resistanceToDecrease.DecreaseValue(decreaseAmount);
        UpdateMobValues();
    }

    private void UpdateMobValues()
    {
        health = maxHealth = GetStatValueByType(StatType.Health) * 10;
        mana = maxMana = GetStatValueByType(StatType.Intelligence) * 10;
        UpdateIndicators();
    }

    public virtual void TakeDamage(MobController damager, float incomingDamage = 1, DamageType damageType = DamageType.BRUTE)
    {
        if (disabled)
        {
            return;
        }
        float finalDamage = 0;

        switch (damageType)
        {
            case DamageType.BRUTE:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.BRUTE);
                break;

            case DamageType.BURN:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.BURN);
                break;

            case DamageType.NATURE:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.NATURE);
                break;

            case DamageType.MAGIC:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.MAGIC);
                break;

            case DamageType.DARK:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.DARK);
                break;

            case DamageType.KARMA:
                finalDamage = incomingDamage - GetResistanceValueByType(ResistanceType.KARMA);
                break;
        }

        finalDamage = Mathf.Round(finalDamage * 100f) / 100f;
        health -= finalDamage;

        UpdateIndicators();

        if (health <= 0 && canDie)
        {
            SoundManager.Instance.PlaySoundOneShot("death", audioSource);
            Death(damager);
            return;
        }

        SoundManager.Instance.PlaySoundOneShot("hitHurt", audioSource);
    }

    public virtual void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        UpdateIndicators();
    }

    public virtual void UpdateIndicators() { }

    public virtual void Death(MobController damager)
    {
        if (disabled)
        {
            return;
        }
        if (damager)
        {
            damager.MobKilled(this);
        }
        if (spawner)
        {
            spawner.RemoveMob(this);
        }
        disabled = true;
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;
        navMeshAgent.enabled = false;
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5);
    }

    public virtual void MobKilled(MobController killedMob)
    {
        IncreaseExperience(killedMob.experienceOnKill);
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
        experience = Mathf.Max(experience - experienceForLevelUp, 0);
        experienceForLevelUp *= 2;
    }

    public virtual bool ApplyBuff(Buff buffToApply)
    {
        if (HasBuff(buffToApply) && !buffToApply.canHaveMultiple)
        {
            Buff existingBuff = GetExistingBuff(buffToApply);
            existingBuff.elapsedTime = 0;
            return true;
        }

        buffs.Add(buffToApply);
        buffToApply.ApplyEffect();
        if (buffToApply.duration <= 0)
        {
            return false;
        }
        StartCoroutine(RemoveBuffAfterDuration(buffToApply));
        return true;
    }

    public virtual void RemoveBuff(Buff buffToRemove)
    {
        buffToRemove.RemoveEffect();
        buffs.Remove(buffToRemove);
    }

    public virtual IEnumerator RemoveBuffAfterDuration(Buff buffToRemove)
    {
        while (buffToRemove.elapsedTime < buffToRemove.duration)
        {
            buffToRemove.elapsedTime += Time.deltaTime;
            yield return null;
        }
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

    public Buff GetExistingBuff(Buff buffToCheck)
    {
        Buff existingBuff = buffs.Find(buff => buff.name == buffToCheck.name);
        if (existingBuff != null)
        {
            return existingBuff;
        }
        return null;
    }

    public void OnEventReceived(GameObject source, EVENT eventReceived)
    {
        if (eventReceived == EVENT.PAUSED)
        {
            OnPauseGame();
        }
        if (eventReceived == EVENT.RESUMED)
        {
            OnResumeGame();
        }
    }

    public void OnPauseGame()
    {
        paused = true;
        if (navMeshAgent)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void OnResumeGame()
    {
        paused = false;
        if (navMeshAgent)
        {
            navMeshAgent.isStopped = false;
        }
    }
}