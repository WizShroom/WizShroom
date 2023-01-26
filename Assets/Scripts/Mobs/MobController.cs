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

    public AudioSource audioSource;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public MobSpawner spawner;
    [HideInInspector] public MobAIController mobAIController;
    [HideInInspector] public bool paused = false;

    [HideInInspector] public bool disabled = false;

    [HideInInspector] public float timeBetweenAttacks = 1f;
    [HideInInspector] public float elapsedAttackTime;

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
    }

    public virtual void Attack(Vector3 attackDirection) { }

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
        mana = maxMana = GetStatValueByType(StatType.Intelligence) * 10;
        UpdateIndicators();
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
        if (damager)
        {
            damager.IncreaseExperience(experienceOnKill);
        }
        if (spawner)
        {
            spawner.RemoveMob(this);
        }
        disabled = true;
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 5);
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