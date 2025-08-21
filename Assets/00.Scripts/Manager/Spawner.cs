using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public GameObject enemyPrefbas;
    public float levelTime;
    int level;
    float timer;
    public GameObject player;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        levelTime = GameManager.Instance.maxGameTime / spawnData.Length;

    }
    void Update()
    {

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / levelTime), spawnData.Length - 1);
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spwan();
        }
    }

    void Spwan()
    {
        Debug.Log("����");
        GameObject enemy = PoolingManager.Instance.GetObject(enemyPrefbas, transform.position, Quaternion.identity);
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level], player);
        
    }
}

[System.Serializable]
public class SpawnData
{
    public int[] spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
