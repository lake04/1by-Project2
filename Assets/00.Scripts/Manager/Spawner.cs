using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public GameObject[] enemyPrefbas;
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
        Debug.Log("½ºÆù");
        int enemyIndex = UnityEngine.Random.Range(0, spawnData[level].enemys.Length);
        GameObject enemy = PoolingManager.Instance.GetObject(enemyPrefbas[enemyIndex], transform.position, Quaternion.identity);
        enemy.transform.position = spawnPoint[UnityEngine.Random.Range(1, spawnPoint.Length)].position;
    
        enemy.GetComponent<EnemyBase>().Init(spawnData[level], player);
        
        

    }
}

[System.Serializable]
public class SpawnData
{
    public int[] enemys;
    public float spawnTime;
    public int health;
    public float speed;
}
