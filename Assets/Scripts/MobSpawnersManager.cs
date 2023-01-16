using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnersManager : MonoBehaviour
{
    public List<MobSpawner> mobSpawners;

    public int maxMobAmount = 30;
    public int currentMobAmount;

    public float maxSpawnDelay = 3;
    public float minSpawnDelay = 1;
    float elapsedTime;

    private void Awake()
    {
        foreach (MobSpawner mobSpawner in mobSpawners)
        {
            mobSpawner.ConnectManager(this);
        }
    }

    private void Update()
    {
        if (currentMobAmount >= maxMobAmount)
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


}
