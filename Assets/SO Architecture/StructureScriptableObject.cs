using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Structure/New Structure")]
public class StructureScriptableObject : ScriptableObject
{
    public string structureName;
    public int lifePoints;
}
