using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private int score;

    public float MovementSpeed { get => movementSpeed; }
    public int Score { get => score; }
}