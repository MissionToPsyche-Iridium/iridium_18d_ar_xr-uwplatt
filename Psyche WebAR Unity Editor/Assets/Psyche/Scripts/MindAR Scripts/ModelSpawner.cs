using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn given model
/// </summary>
public class ModelSpawner : MonoBehaviour
{
    [SerializeField] private float SpawnTime;
    [SerializeField] private GameObject SpawnObject;

    /// <summary>
    /// Invoke spawn at start
    /// </summary>
    void Start()
    {
        Invoke("Spawn", SpawnTime);
    }

    void Spawn()
    {
        Instantiate(SpawnObject, gameObject.transform);
    }
}
