using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawner : MonoBehaviour
{
    public List<GameObject> mobsToSpawn;
    public List<MobController> spawnedMobs;

    public int spawnRadious = 3;

    public string signalToSpawn;

    public MobSpawnersManager ourManager;
    public PlayerController player;

    private void Awake()
    {
        GameSignalHandler.Instance.OnSignalReceived += OnSignalReceived;
    }

    private void OnDestroy()
    {
        GameSignalHandler.Instance.OnSignalReceived -= OnSignalReceived;
    }

    public void ConnectManager(MobSpawnersManager manager)
    {
        ourManager = manager;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public bool Spawn(bool forced = false)
    {

        if (mobsToSpawn.Count <= 0)
        {
            return false;
        }

        if (!forced && player && Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            return false;
        }

        int randomIndex = Random.Range(0, mobsToSpawn.Count);
        GameObject spawnedMob = Instantiate(mobsToSpawn[randomIndex], transform.position, Quaternion.identity);
        spawnedMob.transform.SetParent(transform);

        MobController mobController = spawnedMob.GetComponent<MobController>();

        spawnedMobs.Add(mobController);
        mobController.spawner = this;

        return true;
    }

    public void RemoveMob(MobController mobController)
    {
        if (spawnedMobs.Contains(mobController))
        {
            spawnedMobs.Remove(mobController);
            if (!ourManager)
            {
                return;
            }
            ourManager.DecreaseMobAmount();
        }
    }

    public void OnSignalReceived(GameObject source, string signal)
    {
        if (signal == signalToSpawn)
        {
            Spawn(true);
        }
    }
}
