using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalWorld", menuName = "ScriptableObjects/LocalWorldScriptableObject", order = 1)]
public class LocalWorldScriptableObject : ScriptableObject
{
    public int ID;
    public LocalWorldInfo localWorldInfo;
}
