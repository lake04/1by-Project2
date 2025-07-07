using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int roomCount = 8;
    private Dictionary<Vector2Int, Room> roomMap = new Dictionary<Vector2Int, Room>();

    private readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };

    public GameObject roomPrefab;
    public float roomSpacing = 30f;

    [Header("Player Spawn")]
    public GameObject playerPrefab;
    private Vector2Int startRoomPos = Vector2Int.zero;

    void Start()
    {
        GenerateRooms();
        InstantiateRooms();
        ConnectAllRooms();
    }

    private void GenerateRooms()
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int start = Vector2Int.zero;
        startRoomPos = start;
        queue.Enqueue(start);
        roomMap[start] = null;

        while (roomMap.Count < roomCount && queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;
                if (!roomMap.ContainsKey(next) && Random.value < 0.5f)
                {
                    roomMap[next] = null;
                    queue.Enqueue(next);

                    if (roomMap.Count >= roomCount) break;
                }
            }
        }
    }

    private void InstantiateRooms()
    {
        List<Vector2Int> keys = new List<Vector2Int>(roomMap.Keys);
        foreach (Vector2Int roomPos in keys)
        {
            Vector3 worldPos = new Vector3(roomPos.x * roomSpacing, roomPos.y * roomSpacing, 0f);
            GameObject roomObj = Instantiate(roomPrefab, worldPos, Quaternion.identity);
            Room room = roomObj.GetComponent<Room>();
            roomMap[roomPos] = room;

            bool[] doorInfo = GetDoorInfo(roomPos);
            bool isStartRoom = roomPos == startRoomPos;

            room.GenerateWalls(doorInfo, isStartRoom);  

            if (isStartRoom)
            {
                room.SetStartRoom(true);               
                Instantiate(playerPrefab, worldPos, Quaternion.identity);  
            }
        }
    }



    private bool[] GetDoorInfo(Vector2Int roomPos)
    {
        bool[] doors = new bool[4];
        if (roomMap.ContainsKey(roomPos + Vector2Int.up)) doors[0] = true;
        if (roomMap.ContainsKey(roomPos + Vector2Int.down)) doors[1] = true;
        if (roomMap.ContainsKey(roomPos + Vector2Int.left)) doors[2] = true;
        if (roomMap.ContainsKey(roomPos + Vector2Int.right)) doors[3] = true;
        return doors;
    }

    private void ConnectAllRooms()
    {
        foreach (var kvp in roomMap)
        {
            Vector2Int pos = kvp.Key;
            Room room = kvp.Value;
            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (roomMap.ContainsKey(neighborPos))
                {
                    Room neighborRoom = roomMap[neighborPos];
                    ConnectRoomsWithHallway(room, neighborRoom);
                }
            }
        }
    }

    private void ConnectRoomsWithHallway(Room roomA, Room roomB)
    {
        Vector3 aPos = roomA.transform.position;
        Vector3 bPos = roomB.transform.position;

        Vector3 current = aPos;
        while (Mathf.RoundToInt(current.x) != Mathf.RoundToInt(bPos.x))
        {
            current.x += Mathf.Sign(bPos.x - current.x);
            roomA.SetHallwayTileAtWorld(current);
        }
        while (Mathf.RoundToInt(current.y) != Mathf.RoundToInt(bPos.y))
        {
            current.y += Mathf.Sign(bPos.y - current.y);
            roomA.SetHallwayTileAtWorld(current);
        }
    }
}
