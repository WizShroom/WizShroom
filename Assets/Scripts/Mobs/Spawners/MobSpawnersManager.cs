using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnersManager : MonoBehaviour
{
    public List<MobSpawner> mobSpawners;
    public List<GameObject> mobPrefabs;
    public string spawningMobID;

    bool paused;
    public bool spawnerActive;

    #region Normal Spawn
    [Header("Normal Spawn")]
    public int maxMobAmount = 0;
    public int amountOfEnemiesSpawned = 0;
    public int amountOfEnemiesKilled = 0;
    public int numberOfEnemiesPerLevelUp = 0;
    public int numberOfEnemiesPerLevelUpIncrease = 0;
    public bool infiniteEnemies = false;
    public int remainingEnemiesToSpawn = 99999;
    #endregion

    #region Boss Spawn
    [Header("Boss Spawn")]
    public bool canSpawnBoss = true;
    public int bossAfterMobAmount = 0;
    public int bossAfterMobAmountIncrease = 0;
    public int bossGuardsAmount = 0;
    public int bossGuardsAmountIncrease = 0;

    public int currentBossAmount = 0;
    public bool isBossPresent = false;
    public GameObject bossPrefab;
    public RoomHolder bossRoom;
    #endregion

    private void Awake()
    {
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
        ConnectToSpawners();
    }

    private void OnDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    private void Update()
    {
        if (!spawnerActive || paused)
        {
            return;
        }
        SetupSpawn();
    }

    public void SetupSpawn()
    {
        PrepareSpawnInitialCheck();
    }

    public void ConnectToSpawners()
    {
        foreach (MobSpawner mobSpawner in mobSpawners)
        {
            mobSpawner.ConnectManager(this);
        }
    }

    public void OurMobKilled()
    {
        amountOfEnemiesSpawned--;
        amountOfEnemiesKilled++;
    }

    public void PrepareSpawnInitialCheck()
    {
        if (amountOfEnemiesSpawned >= maxMobAmount || (remainingEnemiesToSpawn <= 0 && !infiniteEnemies))
        {
            return;
        }

        if (canSpawnBoss && !isBossPresent && amountOfEnemiesKilled >= bossAfterMobAmount)
        {
            SpawnBoss();
        }
        /* int storedLevel = mobLevel;
        mobLevel = Mathf.Max(Random.Range(-5, 6) + mobLevel, 1); */
        bool spawned = SpawnMob();
        //mobLevel = storedLevel;
        if (!spawned)
        {
            return;
        }
        PostMobSpawnCheck();
    }

    public void PostMobSpawnCheck()
    {
        amountOfEnemiesSpawned++;
        if (amountOfEnemiesKilled > numberOfEnemiesPerLevelUp)
        {
            //mobLevel += mobLevelIncrease;
            numberOfEnemiesPerLevelUp += numberOfEnemiesPerLevelUpIncrease;
        }
        if (infiniteEnemies)
        {
            return;
        }
        remainingEnemiesToSpawn--;
    }

    public void SpawnBoss()
    {
        if (!isBossPresent)
        {
            isBossPresent = true;
            if (bossRoom && bossRoom.bossSpawnPosition)
            {
                GameObject boss = Instantiate(bossPrefab, bossRoom.bossSpawnPosition.transform.position, Quaternion.identity);
            }
        }
    }

    private bool SpawnMob()
    {
        if (mobSpawners.Count == 0)
        {
            return false;
        }

        List<MobSpawner> spawnersCopy = new List<MobSpawner>(mobSpawners);
        for (int i = 0; i < spawnersCopy.Count; i++)
        {
            MobSpawner temp = spawnersCopy[i];
            int randomIndex = Random.Range(i, spawnersCopy.Count);
            spawnersCopy[i] = spawnersCopy[randomIndex];
            spawnersCopy[randomIndex] = temp;
        }

        int randomSpawner = Random.Range(0, spawnersCopy.Count);
        MobSpawner spawner = spawnersCopy[randomSpawner];
        return spawner.Spawn(true);
    }

    public void OnEventReceived(GameObject source, EVENT receivedEvent)
    {
        if (receivedEvent == EVENT.PAUSED)
        {
            OnPaused();
        }
        if (receivedEvent == EVENT.RESUMED)
        {
            OnResumed();
        }
    }

    public void OnPaused()
    {
        paused = true;
    }

    public void OnResumed()
    {
        paused = false;
    }

    public void SetActive(bool active)
    {
        spawnerActive = active;
    }

}
