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
    public List<MobSpawner> spawners;

    public int currentDepth = 0;
    public int currentYLevel = 0;

    public Vector3 currentGridPosition;
    public bool canGoVertical = false;

}