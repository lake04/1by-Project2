using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap groundTilemap;
    public TileBase groundTile;

    public Tilemap wallTilemap;
    public TileBase wallTile;

    [Header("Auto Wall Tiles")]
    public TileBase wallTop;
    public TileBase wallBottom;
    public TileBase wallLeft;
    public TileBase wallRight;
    public TileBase wallBottomLeft;
    public TileBase wallBottomRight;
    public TileBase wallTopLeft;
    public TileBase wallTopRight;

    [Header("Hallway")]
    public TileBase hallwayTile;
    public Tilemap hallwayTilemap;

    public int width = 30;
    public int height = 20;

    public GameObject doorPrefab;
    private List<GameObject> doorBlockers = new List<GameObject>();

    private EnemySpawner enemySpawner;
    private bool isStartRoom = false;

    public void SetStartRoom(bool isStart)
    {
        isStartRoom = isStart;
    }

    void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    void Start()
    {
        GenerateFloor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isStartRoom)
        {
            foreach (var blocker in doorBlockers)
                blocker.SetActive(true);

            if (enemySpawner != null)
                enemySpawner.TriggerSpawn();
        }
    }

    public void CloseDoors()
    {
        foreach (var blocker in doorBlockers)
            blocker.SetActive(true);
    }

    public void OpenDoors()
    {
        foreach (var blocker in doorBlockers)
            Destroy(blocker);

        doorBlockers.Clear();
    }

    void GenerateFloor()
    {
        for (int x = -width / 2; x <= width / 2; x++)
        {
            for (int y = -height / 2; y <= height / 2; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
    }

    public void GenerateWalls(bool[] doorOpen, bool isStartRoom = false)
    {
        int w = width / 2;
        int h = height / 2;

        for (int x = -w; x <= w; x++)
        {
            Vector3Int pos = new Vector3Int(x, h, 0);
            if (doorOpen[0] && Mathf.Abs(x) <= 1)
            {
                if (!isStartRoom) SpawnDoorBlocker(pos);
            }
            else wallTilemap.SetTile(pos, wallTile);
        }

        for (int x = -w; x <= w; x++)
        {
            Vector3Int pos = new Vector3Int(x, -h, 0);
            if (doorOpen[1] && Mathf.Abs(x) <= 1)
            {
                if (!isStartRoom) SpawnDoorBlocker(pos);
            }
            else wallTilemap.SetTile(pos, wallTile);
        }

        for (int y = -h + 1; y < h; y++)
        {
            Vector3Int pos = new Vector3Int(-w, y, 0);
            if (doorOpen[2] && Mathf.Abs(y) <= 1)
            {
                if (!isStartRoom) SpawnDoorBlocker(pos);
            }
            else wallTilemap.SetTile(pos, wallTile);
        }

        for (int y = -h + 1; y < h; y++)
        {
            Vector3Int pos = new Vector3Int(w, y, 0);
            if (doorOpen[3] && Mathf.Abs(y) <= 1)
            {
                if (!isStartRoom) SpawnDoorBlocker(pos);
            }
            else wallTilemap.SetTile(pos, wallTile);
        }

        for (int x = -w; x <= w; x++)
        {
            for (int y = -h; y <= h; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (wallTilemap.HasTile(pos))
                    AutoSetWallTile(pos);
            }
        }
    }

    private void SpawnDoorBlocker(Vector3Int cellPos)
    {
        Vector3 worldPos = wallTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);

        foreach (var block in doorBlockers)
        {
            if (Vector3.Distance(block.transform.position, worldPos) < 0.1f)
                return;
        }

        GameObject blocker = Instantiate(doorPrefab, worldPos, Quaternion.identity, transform);
        blocker.SetActive(false);
        doorBlockers.Add(blocker);
    }

    private void AutoSetWallTile(Vector3Int pos)
    {
        bool up = wallTilemap.HasTile(pos + Vector3Int.up);
        bool down = wallTilemap.HasTile(pos + Vector3Int.down);
        bool left = wallTilemap.HasTile(pos + Vector3Int.left);
        bool right = wallTilemap.HasTile(pos + Vector3Int.right);

        if (!up && right && wallTopRight != null) wallTilemap.SetTile(pos, wallTopRight);
        else if (!up && left && wallTopLeft != null) wallTilemap.SetTile(pos, wallTopLeft);
        else if (!down && right && wallBottomRight != null) wallTilemap.SetTile(pos, wallBottomRight);
        else if (!down && left && wallBottomLeft != null) wallTilemap.SetTile(pos, wallBottomLeft);
        else if (!up && wallTop != null) wallTilemap.SetTile(pos, wallTop);
        else if (!down && wallBottom != null) wallTilemap.SetTile(pos, wallBottom);
        else if (!left && wallLeft != null) wallTilemap.SetTile(pos, wallLeft);
        else if (!right && wallRight != null) wallTilemap.SetTile(pos, wallRight);
    }

    public void SetHallwayTileAtWorld(Vector3 worldPos)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        groundTilemap.SetTile(cellPos, groundTile);
    }
}
