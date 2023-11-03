using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadedAssets", menuName = "ScriptableObjects/LoadedAssetsScriptableObject", order = 1)]
public class LoadedAssetsScriptableObject : ScriptableObject    
{
    public GameObject BLANK_TILE_PREFAB;
    public GameObject DARK_DIRT_TILE_PREFAB;
    public GameObject DEEP_WATER_TILE_PREFAB;
    public GameObject SHALLOW_WATER_TILE_PREFAB;
    public GameObject SAND_TILE_PREFAB;
}
