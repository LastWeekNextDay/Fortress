using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;
    [SerializeField]private LoadedAssetManager _loadedAssetManager;
    
    private void Awake()
    {
        _loadedAssetManager = new LoadedAssetManager(ScriptableObject.CreateInstance<LoadedAssetsScriptableObject>());
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("AssetManager already exists!");
            Destroy(this.gameObject);
        }
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position)
    {
        Debug.Log("Spawning " + prefab.name + " at " + position);
        return Instantiate(prefab, position, prefab.transform.rotation);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Transform parent)
    {
        Debug.Log("Spawning " + prefab.name + " at " + position + " with parent " + parent.name);
        return Instantiate(prefab, position, prefab.transform.rotation, parent);
    }

    public GameObject GetTile(TileType type)
    {
        Debug.Log("Getting tile " + type);
        _loadedAssetManager.LoadTileToMemoryIfNeeded(type);
        switch (type)
        {
            case TileType.BLANK:
                return _loadedAssetManager.loadedAssets.BLANK_TILE;
            case TileType.DIRT_DARK:
                return _loadedAssetManager.loadedAssets.DARK_DIRT_TILE;
            case TileType.DEEP_WATER:
                return _loadedAssetManager.loadedAssets.DEEP_WATER_TILE;
            case TileType.SHALLOW_WATER:
                return _loadedAssetManager.loadedAssets.SHALLOW_WATER_TILE;
            case TileType.SAND:
                return _loadedAssetManager.loadedAssets.SAND_TILE;
            case TileType.GRASS:
                return _loadedAssetManager.loadedAssets.GRASS_TILE;
            case TileType.STONE:
                return _loadedAssetManager.loadedAssets.STONE_TILE;
            default:
                Debug.LogError("Tile type " + type + " not found!");
                return null;
        }
    }
}
