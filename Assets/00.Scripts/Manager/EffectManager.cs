using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] List<ParticlePrefab> particlePrefabs;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Effects(int ID,Transform pos)
    {
        GameObject effect = Instantiate(particlePrefabs[ID].prefab,pos);
    }
}
