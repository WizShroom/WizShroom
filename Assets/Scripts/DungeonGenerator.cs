using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    public Direction generatingDirection;
    public List<GameObject> roomPrefabs;
    public int width = 10;
    public int heigth = 10;

    public NavMeshSurface surface;

    private void Start()
    {
        GameObject holder = new GameObject("Holder");
        for (int x = -width / 2; x <= width / 2; x++)
        {
            for (int z = -heigth / 2; z < heigth / 2; z++)
            {
                Vector3 roomPosition = new Vector3(x * 14, 0, z * 14);
                GameObject newRoomToPlace = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
                int rotation = Random.Range(0, 4) * 90;
                Quaternion rot = Quaternion.Euler(0, rotation, 0);
                GameObject placedRoom = Instantiate(newRoomToPlace, roomPosition, rot);
                placedRoom.transform.SetParent(holder.transform);
                placedRoom.isStatic = true;
            }
        }

        holder.transform.position = GetVectorFromDirection(generatingDirection) * width / 2 + transform.position - GetVectorFromDirection(generatingDirection) * 0.5f;

        surface.BuildNavMesh();
    }

    Vector3 GetVectorFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return new Vector3(14, 0, 0);
            case Direction.SOUTH:
                return new Vector3(-14, 0, 0);
            case Direction.EAST:
                return new Vector3(0, 0, -14);
            case Direction.WEST:
                return new Vector3(0, 0, 14);
            default:
                return Vector3.zero;
        }
    }

}
public enum Direction
{
    NORTH,
    SOUTH,
    EAST,
    WEST,
}