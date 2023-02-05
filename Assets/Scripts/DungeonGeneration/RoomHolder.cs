using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHolder : MonoBehaviour
{
    public DIRECTION connections;
    public int roomSize = 16;
    public List<RoomHolder> connectedRooms = new List<RoomHolder>();
    public List<RoomConnection> roomConnectors;
    public List<RoomSeal> roomSeals;
    public Transform roomLanding;
    public List<MobSpawner> spawners;
    public List<PropSpawner> propSpawners;
    public List<GameObject> chests;

    public GameObject bossSpawnPosition;

    public int currentDepth = 0;
    public int currentYLevel = 0;

    public Vector3 currentGridPosition;
    public bool canGoVertical = false;
    public bool canRotate = false;

    public bool initialRoom = false;
    public bool treasureRoom = false;
    public bool bossRoom = false;

    public RoomConnection GetConnectionByDir(DIRECTION dir)
    {
        RoomConnection returnConnection = null;

        foreach (RoomConnection roomConnection in roomConnectors)
        {
            if (roomConnection.ourDir == dir)
            {
                returnConnection = roomConnection;
                break;
            }
        }

        return returnConnection;
    }

    public void SpawnMobs()
    {
        if (initialRoom || treasureRoom || bossRoom)
        {
            return;
        }
        for (int i = 0; i < Mathf.Clamp(currentDepth + Random.Range(-5, 0), 1, 4); i++)
        {
            MobSpawner mobSpawner = spawners[Random.Range(0, spawners.Count)];
            mobSpawner.Spawn(true);
        }
    }

    public void SpawnDestroyables()
    {
        foreach (PropSpawner propSpawner in propSpawners)
        {
            if (Random.Range(0, 101) > 20)
            {
                continue;
            }
            propSpawner.SpawnProp();
        }
    }

    public void SpawnTreasures()
    {
        if (chests.Count == 0)
        {
            return;
        }

        GameObject pickedChest = chests[Random.Range(0, chests.Count)];
        List<GameObject> toDelete = new List<GameObject>();
        foreach (GameObject chest in chests)
        {
            if (chest == pickedChest)
            {
                continue;
            }

            toDelete.Add(chest);
        }

        foreach (GameObject chestToDelete in toDelete)
        {
            Destroy(chestToDelete);
        }
    }

}