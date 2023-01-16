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

    public bool Spawn()
    {

        if (Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            return false;
        }

        Vector3 resultPosition = default(Vector3);
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadious + transform.position;
            randomPosition.y = 0;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 0.3f, NavMesh.AllAreas))
            {
                resultPosition = randomPosition;
                break;
            }
        }

        if (resultPosition == default(Vector3))
        {
            return false;
        }

        int randomIndex = Random.Range(0, mobsToSpawn.Count);
        GameObject spawnedMob = Instantiate(mobsToSpawn[randomIndex], resultPosition, Quaternion.identity);

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
            ourManager.DecreaseMobAmount();
        }
    }

    public void OnSignalReceived(GameObject source, string signal)
    {
        if (signal == signalToSpawn)
        {
            Spawn();
        }
    }
}
