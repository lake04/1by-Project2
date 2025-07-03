using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int roomCount = 8;
    private Dictionary<Vector2Int, bool> roomMap = new Dictionary<Vector2Int, bool>();

    private readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
    };

    public GameObject roomPrefab;
    public float roomSpacing =10f;


    void Start()
    {
        GenerateRooms();
        InstantiateRooms();
    }

    private void GenerateRooms()
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int start = Vector2Int.zero;
        queue.Enqueue(start);
        roomMap[start] = true;

        while(roomMap.Count < roomCount && queue.Count >0)
        {
            Vector2Int current = queue.Dequeue();

            foreach(var dir in directions)
            {
                Vector2Int next = current + dir;
                if(!roomMap.ContainsKey(next) && Random.value <0.5f)
                {
                    roomMap[next] = true;
                    queue.Enqueue(next);

                    if (roomMap.Count >= roomCount) break;

                }
            }
        }

        foreach(var room in roomMap.Keys)
        {
            Debug.Log($"{room}");
        }
    }

    private void InstantiateRooms()
    {
        foreach(var roomPos in roomMap.Keys)
        {
            Vector3 worldPos = new Vector3(roomPos.x * roomSpacing, roomPos.y * roomSpacing, 0f);
            GameObject roomObj = Instantiate(roomPrefab, worldPos, Quaternion.identity);

            Room room = roomObj.GetComponent<Room>();

            bool[] doorInfo = GetDoorInfo(roomPos);
            room.GenerateWalls(doorInfo);
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
}
