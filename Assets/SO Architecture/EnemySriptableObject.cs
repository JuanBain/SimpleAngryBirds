using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/NewEnemy")]
public class EnemySriptableObject : ScriptableObject
{
    public string enemyName;
    public int lifePoints;
    public int pointsGivenWhenHit;
    public int pointsGivenWhenDestroyed;
}
