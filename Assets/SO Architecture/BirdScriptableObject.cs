using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Bird/NewBird")]
public class BirdScriptableObject : ScriptableObject
{
    public string birdName;
    public bool explosiveBird;
    public float explotionDamage;
    public bool multipleBird;
    public float destructionTime;
    public GameObject explotion;
}
