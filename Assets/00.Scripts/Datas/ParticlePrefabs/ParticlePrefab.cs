using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/ParticlePrefab", fileName ="ParticlePrefab")]
public class ParticlePrefab : ScriptableObject
{
    public string name;
    public int ID;
    public GameObject prefab;
}
