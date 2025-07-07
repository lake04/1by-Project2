using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public int spawnCount = 3;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Room room;

    private bool isTriggered = false;

    void Start()
    {
        room = GetComponent<Room>();
    }

    public void TriggerSpawn()
    {
        if (isTriggered) return;
        isTriggered = true;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

        room.CloseDoors(); 

        StartCoroutine(CheckEnemiesDead());
    }

    private System.Collections.IEnumerator CheckEnemiesDead()
    {
        while (true)
        {
            spawnedEnemies.RemoveAll(e => e == null);

            if (spawnedEnemies.Count == 0)
            {
                room.OpenDoors(); 
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
