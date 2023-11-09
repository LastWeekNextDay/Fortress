using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadedAssets", menuName = "ScriptableObjects/LoadedAssetsScriptableObject", order = 1)]
public class LoadedAssetsScriptableObject : ScriptableObject    
{
    public GameObject BLANK_TILE;
    public GameObject DARK_DIRT_TILE;
    public GameObject DEEP_WATER_TILE;
    public GameObject SHALLOW_WATER_TILE;
    public GameObject SAND_TILE;
    public GameObject GRASS_TILE;
    public GameObject STONE_TILE;
}
