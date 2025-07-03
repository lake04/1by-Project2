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

    public int width = 12;
    public int height = 8;

    public GameObject doorPrefab;

    private List<GameObject> doorBlockkers = new List<GameObject>();


    void Start()
    {
        GenerateFloor();
    }

   void GenerateFloor()
    {
        for(int x= -width /2; x<=width/2; x++)
        {
            for(int y= -height /2; y<=height/2; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
    }

    public void GenerateWalls(bool[] doorOpen)
    {
        int w = width / 2;
        int h = height / 2;

        for (int x = -w; x <= w; x++)
        {
            Vector3Int pos = new Vector3Int(x, h, 0);
            if (doorOpen[0] && Mathf.Abs(x) <= 1)
                SpawnDoorBlocker(pos); 
            else
                wallTilemap.SetTile(pos, wallTile); 
        }

        for (int x = -w; x <= w; x++)
        {
            Vector3Int pos = new Vector3Int(x, -h, 0);
            if (doorOpen[1] && Mathf.Abs(x) <= 1)
                SpawnDoorBlocker(pos);
            else
                wallTilemap.SetTile(pos, wallTile);
        }

        for (int y = -h + 1; y < h; y++)
        {
            Vector3Int pos = new Vector3Int(-w, y, 0);
            if (doorOpen[2] && Mathf.Abs(y) <= 1)
                SpawnDoorBlocker(pos);
            else
                wallTilemap.SetTile(pos, wallTile);
        }

        for (int y = -h + 1; y < h; y++)
        {
            Vector3Int pos = new Vector3Int(w, y, 0);
            if (doorOpen[3] && Mathf.Abs(y) <= 1)
                SpawnDoorBlocker(pos);
            else
                wallTilemap.SetTile(pos, wallTile);
        }
    }

    private void SpawnDoorBlocker(Vector3Int cellPos)
    {
        Vector3 worldPos = wallTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f);

        foreach( var block in doorBlockkers)
        {
            if (Vector3.Distance(block.transform.position, worldPos) < 0.1f) return;
        }
        GameObject blocker = Instantiate(doorPrefab, worldPos, Quaternion.identity, transform);
        doorBlockkers.Add(blocker);
    }

    public void OpenDoors()
    {
        foreach(var blocker in doorBlockkers)
        {
            Destroy(blocker);
        }
        doorBlockkers.Clear();
    }
}
