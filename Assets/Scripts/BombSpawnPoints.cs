using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BombSpawnPoints", menuName = "ScriptableObjects/BombSpawnPoints")]
public class BombSpawnPoints : ScriptableObject
{
    public Vector2Int[] spawnPoints;
}