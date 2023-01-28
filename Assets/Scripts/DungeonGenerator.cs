using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : SingletonMono<DungeonGenerator>
{
    public DIRECTION initialDirection;

    public List<GameObject> roomsPrefab;
    public List<GameObject> verticalRoomsPrefab;

    public GameObject startingRoom;

    public List<RoomHolder> placedRooms;

    public List<Vector3> gridPositions;

    public int maxDepth = 3;
    public int maxHeight = 1;

    public int maxSize = 5;

    public bool testing = false;

    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        if (testing)
        {
            StartCoroutine(GenerateDungeon());
        }
    }

    public IEnumerator GenerateDungeon()
    {
        GameObject startingRoomEntity = Instantiate(startingRoom, gameObject.transform);
        RoomHolder startingRoomHolder = startingRoomEntity.GetComponent<RoomHolder>();
        startingRoomHolder.connections |= GetOppositeDirection(initialDirection);
        Vector3 roomPosition = DirectionToVector3(initialDirection) * startingRoomHolder.roomSize * 0.5f;
        startingRoomHolder.transform.position += roomPosition;
        placedRooms.Add(startingRoomHolder);
        startingRoomHolder.currentGridPosition = Vector3.zero;

        gridPositions.Add(startingRoomHolder.currentGridPosition);

        Task branch = new Task(BranchOut(startingRoomHolder));

        while (branch.Running)
        {
            yield return null;
        }

        Task close = new Task(CloseDungeon());
        while (close.Running)
        {
            yield return null;
        }
    }


    public IEnumerator BranchOut(RoomHolder startingRoomHolder)
    {
        yield return null;
        Vector3 initialPosition = startingRoomHolder.transform.position;
        List<DIRECTION> availableDirections = GetAvailableDirections(startingRoomHolder.connections);
        if (Random.Range(0, 101) > 98)
        {
            availableDirections.Remove(availableDirections[Random.Range(0, availableDirections.Count)]);
        }
        for (int i = 0; i < availableDirections.Count; i++)
        {
            DIRECTION temp = availableDirections[i];
            int randomIndex = Random.Range(i, availableDirections.Count);
            availableDirections[i] = availableDirections[randomIndex];
            availableDirections[randomIndex] = temp;
        }
        foreach (DIRECTION direction in availableDirections)
        {
            bool verticalDirection = false;
            if (direction == DIRECTION.UP || direction == DIRECTION.DOWN)
            {
                if (!startingRoomHolder.canGoVertical)
                {
                    continue;
                }
                verticalDirection = true;
            }

            Vector3 nextGridPosition = startingRoomHolder.currentGridPosition + DirectionToVector3(direction);

            int xDirection = IsZDirection(initialDirection) ? 1 : 2;
            int zDirection = IsZDirection(initialDirection) ? 2 : 1;

            float distance = Mathf.Max(Mathf.Abs(nextGridPosition.x) / xDirection, Mathf.Abs(nextGridPosition.z) / zDirection);

            if (Mathf.Abs(nextGridPosition.y) > maxHeight)
            {
                continue;
            }

            if (gridPositions.Contains(nextGridPosition) || distance > maxSize / 2 || Random.Range(0, 101) > 98 || !IsCorrectDirection(initialDirection, Vector3.zero, nextGridPosition))
            {
                continue;
            }

            gridPositions.Add(nextGridPosition);

            Vector3 nextPosition = startingRoomHolder.transform.position + DirectionToVector3(direction) * startingRoomHolder.roomSize;
            GameObject nextRoomEntry = null;
            if (verticalDirection)
            {
                nextRoomEntry = Instantiate(verticalRoomsPrefab[Random.Range(0, verticalRoomsPrefab.Count)], gameObject.transform);
            }
            else
            {
                nextRoomEntry = Instantiate(roomsPrefab[Random.Range(0, roomsPrefab.Count)], gameObject.transform);
            }
            nextRoomEntry.transform.position = nextPosition;
            RoomHolder nextRoomHolder = nextRoomEntry.GetComponent<RoomHolder>();
            placedRooms.Add(nextRoomHolder);

            RandomRoomRotation(nextRoomHolder);

            startingRoomHolder.connectedRooms.Add(nextRoomHolder);
            nextRoomHolder.connectedRooms.Add(startingRoomHolder);

            if (verticalDirection)
            {
                startingRoomHolder.GetConnectionByDir(direction).connectedRoom = nextRoomHolder;
                nextRoomHolder.GetConnectionByDir(GetOppositeDirection(direction)).connectedRoom = startingRoomHolder;
            }

            startingRoomHolder.connections |= direction;
            nextRoomHolder.connections |= GetOppositeDirection(direction);

            nextRoomHolder.currentDepth = startingRoomHolder.currentDepth + 1;
            nextRoomHolder.currentGridPosition = nextGridPosition;

            if (nextRoomHolder.currentDepth < maxDepth)
            {
                Task newBranch = new Task(BranchOut(nextRoomHolder));
                while (newBranch.Running)
                {
                    yield return null;
                }
            }
        }
    }

    public IEnumerator CloseDungeon()
    {
        foreach (RoomHolder room in placedRooms)
        {
            List<RoomConnection> connectorsToDelete = new List<RoomConnection>();
            foreach (RoomConnection connector in room.roomConnectors)
            {
                if (!room.connections.HasFlag(connector.ourDir))
                {
                    connectorsToDelete.Add(connector);
                }
            }
            foreach (RoomConnection connector in connectorsToDelete)
            {
                room.roomConnectors.Remove(connector);
                // you can also delete the connector object here
                Destroy(connector.gameObject);
            }

            yield return null;

            List<RoomSeal> sealsToDelete = new List<RoomSeal>();
            foreach (RoomSeal seal in room.roomSeals)
            {
                if (room.connections.HasFlag(seal.ourDir))
                {
                    sealsToDelete.Add(seal);
                }
            }
            foreach (RoomSeal seal in sealsToDelete)
            {
                room.roomSeals.Remove(seal);
                // you can also delete the connector object here
                Destroy(seal.gameObject);
            }
        }
    }

    public IEnumerator PopulateDungeon()
    {
        foreach (RoomHolder room in placedRooms)
        {
            if (room.spawners.Count <= 0)
            {
                continue;
            }

            for (int i = 0; i < Mathf.Clamp(room.currentDepth + Random.Range(-5, 0), 1, 4); i++)
            {
                MobSpawner mobSpawner = room.spawners[Random.Range(0, room.spawners.Count)];
                if (!mobSpawner.Spawn(true))
                {
                    Debug.Log(mobSpawner + " Help");
                }
            }
            yield return null;
        }
    }

    public void RandomRoomRotation(RoomHolder roomToRotate)
    {
        int randomRotation = Random.Range(0, 4);

        randomRotation *= 90;

        foreach (RoomConnection connector in roomToRotate.roomConnectors)
        {
            connector.ourDir = RotateDirectionClockWise(randomRotation, connector.ourDir);
        }
        foreach (RoomSeal seal in roomToRotate.roomSeals)
        {
            seal.ourDir = RotateDirectionClockWise(randomRotation, seal.ourDir);
        }

        roomToRotate.transform.rotation = Quaternion.Euler(0, randomRotation, 0);
    }

    public DIRECTION GetOppositeDirection(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.NORTH:
                return DIRECTION.SOUTH;
            case DIRECTION.SOUTH:
                return DIRECTION.NORTH;
            case DIRECTION.EAST:
                return DIRECTION.WEST;
            case DIRECTION.WEST:
                return DIRECTION.EAST;
            case DIRECTION.UP:
                return DIRECTION.DOWN;
            case DIRECTION.DOWN:
                return DIRECTION.UP;
            default:
                return DIRECTION.NORTH;
        }
    }

    public Vector3 DirectionToVector3(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.NORTH:
                return new Vector3(0, 0, 1);
            case DIRECTION.SOUTH:
                return new Vector3(0, 0, -1);
            case DIRECTION.EAST:
                return new Vector3(1, 0, 0);
            case DIRECTION.WEST:
                return new Vector3(-1, 0, 0);
            case DIRECTION.UP:
                return new Vector3(0, 1, 0);
            case DIRECTION.DOWN:
                return new Vector3(0, -1, 0);
            default:
                return Vector3.zero;
        }
    }

    public List<DIRECTION> GetAvailableDirections(DIRECTION takenDirection)
    {
        DIRECTION allDirections = DIRECTION.NORTH | DIRECTION.SOUTH | DIRECTION.EAST | DIRECTION.WEST | DIRECTION.UP | DIRECTION.DOWN;
        DIRECTION availableDirections = allDirections & ~takenDirection;
        List<DIRECTION> availableDirectionsList = new List<DIRECTION>();
        foreach (DIRECTION direction in DIRECTION.GetValues(typeof(DIRECTION)))
        {
            if ((availableDirections & direction) == direction)
            {
                availableDirectionsList.Add(direction);
            }
        }
        return availableDirectionsList;

    }

    public bool IsZDirection(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.NORTH:
            case DIRECTION.SOUTH:
                return true;
            case DIRECTION.EAST:
            case DIRECTION.WEST:
                return false;
            default:
                return false;
        }
    }

    public bool IsCorrectDirection(DIRECTION direction, Vector3 defaultPosition, Vector3 nextPosition)
    {
        switch (direction)
        {
            case DIRECTION.NORTH:
                return nextPosition.z >= defaultPosition.z;
            case DIRECTION.SOUTH:
                return nextPosition.z <= defaultPosition.z;
            case DIRECTION.EAST:
                return nextPosition.x >= defaultPosition.x;
            case DIRECTION.WEST:
                return nextPosition.x <= defaultPosition.x;
            case DIRECTION.UP:
                return nextPosition.y >= defaultPosition.y;
            case DIRECTION.DOWN:
                return nextPosition.y <= defaultPosition.y;
            default:
                return false;
        }
    }

    public DIRECTION RotateDirectionClockWise(int rotationAmount, DIRECTION directionToSet)
    {
        switch (rotationAmount)
        {
            case 90:
                if (directionToSet == DIRECTION.NORTH)
                {
                    return DIRECTION.EAST;
                }
                else if (directionToSet == DIRECTION.EAST)
                {
                    return DIRECTION.SOUTH;
                }
                else if (directionToSet == DIRECTION.SOUTH)
                {
                    return DIRECTION.WEST;
                }
                else if (directionToSet == DIRECTION.WEST)
                {
                    return DIRECTION.NORTH;
                }
                break;

            case 180:
                if (directionToSet == DIRECTION.NORTH)
                {
                    return DIRECTION.SOUTH;
                }
                else if (directionToSet == DIRECTION.EAST)
                {
                    return DIRECTION.WEST;
                }
                else if (directionToSet == DIRECTION.SOUTH)
                {
                    return DIRECTION.NORTH;
                }
                else if (directionToSet == DIRECTION.WEST)
                {
                    return DIRECTION.EAST;
                }
                break;

            case 270:
                if (directionToSet == DIRECTION.NORTH)
                {
                    return DIRECTION.WEST;
                }
                else if (directionToSet == DIRECTION.EAST)
                {
                    return DIRECTION.NORTH;
                }
                else if (directionToSet == DIRECTION.SOUTH)
                {
                    return DIRECTION.EAST;
                }
                else if (directionToSet == DIRECTION.WEST)
                {
                    return DIRECTION.SOUTH;
                }
                break;
        }
        return directionToSet;
    }

}

[System.Flags]
public enum DIRECTION
{
    NORTH = 1 << 0,
    SOUTH = 1 << 1,
    EAST = 1 << 2,
    WEST = 1 << 3,
    UP = 1 << 4,
    DOWN = 1 << 5,
}
