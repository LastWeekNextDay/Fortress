using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
using UnityEngine.AddressableAssets;

public class LocalWorldBuilder : MonoBehaviour
{
    public LoadedAssetsScriptableObject loadedAssets;
    public static LocalWorldBuilder Instance;
    LocalWorld _localWorld;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("LocalWorldBuilder already exists!");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _localWorld = GameObject.Find("LocalWorld").GetComponent<LocalWorld>();
    }

    public void GenerateLocalWorld()
    {
        Debug.Log("Generating local world...");
        // later we save info to localworldinfo
        // here we generate seed from clicked tile if no info is found, or get seed from localworldinfo
        // then we get size of world from localworldinfo
        _localWorld.TileCountX = 500;
        _localWorld.TileCountY = 500;
        _localWorld.Tiles = new GameObject[_localWorld.TileCountX, _localWorld.TileCountY];
        FillTiles();
    }

    void FillTiles()
    {
        Debug.Log("Filling tiles...");
        // create an empty gameobject called Tiles and put it under LocalWorld
        GameObject tiles = new GameObject("Tiles");
        tiles.transform.parent = GameObject.Find("LocalWorld").transform;
        int id_counter = 0;
        float[,] noiseValues = Noise.Calc2D(_localWorld.TileCountX, _localWorld.TileCountY, 0.1f);
        for (int y = 0; y < _localWorld.TileCountX; y++)
        {
            for (int x = 0; x < _localWorld.TileCountY; x++)
            {
                GameObject tile = GetTileThroughNoiseValue(noiseValues[x, y]);
                if (tile == null)
                {
                    continue;
                }
                _localWorld.Tiles[x, y] = AssetManager.Instance.Spawn(tile, new Vector3(x, y, 0), tiles.transform);
                _localWorld.Tiles[x, y].GetComponent<Tile>().ID = id_counter;
                _localWorld.Tiles[x, y].name = TileType.BLANK.ToString() + id_counter;
                id_counter++;
            }
        }
    }

    GameObject GetTileThroughNoiseValue(float noiseValue)
    {
        int noiseValueConverted = Mathf.RoundToInt(noiseValue);
        switch (noiseValueConverted)
        {
            case int n when (n <= 50):
                return loadedAssets.BLANK_TILE_PREFAB;
        }
        return null;
    }
}
