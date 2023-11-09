using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class LoadedAssetManager
{
    public LoadedAssetsScriptableObject loadedAssets;

    public LoadedAssetManager(LoadedAssetsScriptableObject loadedAssets)
    {
        this.loadedAssets = loadedAssets;
    }

    public GameObject LoadTileToMemory(TileType type)
    {
        Debug.Log("Loading tile " + type);
        switch (type)
        {
            case TileType.BLANK:
                return LoadAssetToMemory<GameObject>("BLANK_TILE");
            case TileType.DIRT_DARK:
                return LoadAssetToMemory<GameObject>("DARK_DIRT_TILE");
            case TileType.DEEP_WATER:
                return LoadAssetToMemory<GameObject>("DEEP_WATER_TILE");
            case TileType.SHALLOW_WATER:
                return LoadAssetToMemory<GameObject>("SHALLOW_WATER_TILE");
            case TileType.SAND:
                return LoadAssetToMemory<GameObject>("SAND_TILE");
            case TileType.GRASS:
                return LoadAssetToMemory<GameObject>("GRASS_TILE");
            case TileType.STONE:
                return LoadAssetToMemory<GameObject>("STONE_TILE");
            default:
                Debug.LogError("Tile type " + type + " not found!");
                return null;
        }
    }

    public void LoadTileToMemoryIfNeeded(TileType type)
    {
        switch (type)
        {
            case TileType.BLANK:
                if (loadedAssets.BLANK_TILE == null)
                {
                    Debug.Log(type + " not in memory. Loading...");
                    loadedAssets.BLANK_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.DIRT_DARK:
                if (loadedAssets.DARK_DIRT_TILE == null)
                {
                    Debug.Log(type + " not in memory. Loading...");
                    loadedAssets.DARK_DIRT_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.DEEP_WATER:
                if (loadedAssets.DEEP_WATER_TILE == null)
                {
                    Debug.Log(type + " not in memory. Loading...");
                    loadedAssets.DEEP_WATER_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.SHALLOW_WATER:
                if (loadedAssets.SHALLOW_WATER_TILE == null)
                {
                    Debug.Log(type +  "not in memory. Loading...");
                    loadedAssets.SHALLOW_WATER_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.SAND:
                if (loadedAssets.SAND_TILE == null)
                {
                    Debug.Log(type + " not in memory. Loading...");
                    loadedAssets.SAND_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.GRASS:
                if (loadedAssets.GRASS_TILE == null)
                {
                    Debug.Log(type + " not in memory. Loading...");
                    loadedAssets.GRASS_TILE = LoadTileToMemory(type);
                }
                break;
            case TileType.STONE:
                if (loadedAssets.STONE_TILE == null)
                {
                    Debug.Log(type + "  not in memory. Loading...");
                    loadedAssets.STONE_TILE = LoadTileToMemory(type);
                }
                break;
            default:
                Debug.LogError("Tile type " + type + " not found!");
                break;
        }
    }

    T LoadAssetToMemory<T>(string name)
    {
        Debug.Log("Loading asset " + name);
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);
        handle.WaitForCompletion();
        return handle.Result;
    }
}
