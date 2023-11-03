using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;
    
    private void Awake()
    {
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        Debug.Log("Spawning " + prefab.name + " at " + position);
        return Instantiate(prefab, position, prefab.transform.rotation);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Transform parent)
    {
        Debug.Log("Spawning " + prefab.name + " at " + position + " with parent " + parent.name);
        return Instantiate(prefab, position, prefab.transform.rotation, parent);
    }

    public T LoadAsset<T>(string name)
    {
        Debug.Log("Loading asset " + name);
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);
        handle.WaitForCompletion();
        return handle.Result;
    }

    public GameObject LoadTile(TileType type)
    {
        Debug.Log("Loading tile " + type);
        switch (type)
        {
            case TileType.BLANK:
                return LoadAsset<GameObject>("BLANK_TILE");
            case TileType.DIRT_DARK:
                return LoadAsset<GameObject>("DARK_DIRT_TILE");
            case TileType.DEEP_WATER:
                return LoadAsset<GameObject>("DEEP_WATER_TILE");
            case TileType.SHALLOW_WATER:
                return LoadAsset<GameObject>("SHALLOW_WATER_TILE");
            case TileType.SAND:
                return LoadAsset<GameObject>("SAND_TILE");
            default:
                Debug.LogError("Tile type " + type + " not found!");
                return null;
        }
    }
}
