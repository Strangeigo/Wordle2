using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewBonus", menuName = "Game/Bonus")]
public class BonusSO : ScriptableObject
{
    public string id;
    public string bonusName;
    public string description;
    [Range(0f, 1f)] public float spawnRate;
}
