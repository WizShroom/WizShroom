using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnersManager : MonoBehaviour
{
    public List<MobSpawner> mobSpawners;

    public int maxMobAmount = 30;
    public int currentMobAmount;
    public string spawningMobID;

    public float maxSpawnDelay = 3;
    public float minSpawnDelay = 1;
    float elapsedTime;

    bool paused;

    private void Awake()
    {
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
        foreach (MobSpawner mobSpawner in mobSpawners)
        {
            mobSpawner.ConnectManager(this);
        }
    }

    private void OnDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    private void Update()
    {
        if (currentMobAmount >= maxMobAmount || paused)
        {
            return;
        }

        if (elapsedTime > Random.Range(minSpawnDelay, maxSpawnDelay))
        {
            MobSpawner pickedSpawner = mobSpawners[Random.Range(0, mobSpawners.Count)];
            if (!pickedSpawner.Spawn())
            {
                return;
            }
            currentMobAmount++;
            elapsedTime = 0;
        }
        elapsedTime += Time.deltaTime;
    }

    public void DecreaseMobAmount()
    {
        currentMobAmount -= 1;
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

}
