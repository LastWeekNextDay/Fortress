using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
using UnityEngine.AddressableAssets;
using System.IO;

public class LocalWorldBuilder : MonoBehaviour
{
    public LoadedAssetsScriptableObject loadedAssets;
    public static LocalWorldBuilder Instance;

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
        
    }

    public void BuildLocalWorld()
    {
        Debug.Log("Building local world...");
        LocalWorld localWorld = GameObject.Find("LocalWorld").GetComponent<LocalWorld>();
        if (localWorld == null)
        {
            throw new Exception("LocalWorld object not initialized.");
        }
        localWorld.TileCountX = PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX;
        localWorld.TileCountY = PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY;
        localWorld.Tiles = new GameObject[localWorld.TileCountX, localWorld.TileCountY];
        FillTiles();
    }

    void FillTiles()
    {
        Debug.Log("Filling tiles...");
        LocalWorld localWorld = GameObject.Find("LocalWorld").GetComponent<LocalWorld>();
        if (CheckIfGenerationNeeded())
        {
            Debug.Log("Tiles not found. Generating map tiles...");
            GenerateTiles();
        }
        localWorld.Tiles = LoadTiles();
    }

    GameObject[,] LoadTiles()
    {
        Debug.Log("Loading tiles...");
        string tilesFilePath = Path.Combine(
                       Application.dataPath,
                       "World/Instances",
                       PersistentSessionInformation.Instance.loadedGlobalWorld.Key.ToString(),
                       PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString(),
                       "Tiles.json");
        string json = File.ReadAllText(tilesFilePath);
        TileDataWrapper tileDataWrapper = JsonUtility.FromJson<TileDataWrapper>(json);
        TileInfo[] tiles = tileDataWrapper.Tiles;
        Debug.Log("Tiles loaded: " + tiles);
        TileInfo[,] tilesConvert = new TileInfo[PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX, 
                                                PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY];
        for (int y = 0; y < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY; y++)
        {
            for (int x = 0; x < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX; x++)
            {
                foreach(TileInfo tileInfo in tiles)
                {
                    if (tileInfo.PositionX == x && tileInfo.PositionY == y)
                    {
                        tilesConvert[x, y] = tileInfo;
                    }
                }
            }
        }
        GameObject tilesObj = new GameObject("Tiles");
        tilesObj.transform.parent = GameObject.Find("LocalWorld").transform;
        GameObject[,] tilesGameObjects = new GameObject[PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX,
                                                        PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY];
        for (int y = 0; y < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY; y++)
        {
            for (int x = 0; x < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX; x++)
            {
                TileInfo tileInfo = GetTileInfoOfCoordinates(x, y, tilesConvert);
                if (tileInfo == null)
                {
                    continue;
                }
                AssetManager assetManager = GameObject.Find("AssetManager").GetComponent<AssetManager>();
                GameObject tile = assetManager.LoadTile(tileInfo.Type);
                tilesGameObjects[x, y] = assetManager.Spawn(tile, new Vector3(x, y, 0), tilesObj.transform);
                tilesGameObjects[x, y].GetComponent<Tile>().TileInfo = tileInfo;
                tilesGameObjects[x, y].name = tileInfo.Type.ToString() + tileInfo.ID;
            }
        }
        return tilesGameObjects;
    }

    TileInfo GetTileInfoOfCoordinates(int x, int y, TileInfo[,] tiles)
    {
        Debug.Log("Getting tile info of coordinates " + x + ", " + y);
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j].PositionY == y && tiles[i, j].PositionX == x)
                {
                    return tiles[i, j];
                }
            }
        }
        return null;
    }

    private void GenerateTiles()
    {
        Debug.Log("Generating tiles...");
        float[,] noiseValues = GenerateLocalMapNoiseValues(0.01f);
        string localWorldPath = Path.Combine(
                       Application.dataPath,
                       "World/Instances",
                       PersistentSessionInformation.Instance.loadedGlobalWorld.Key.ToString(),
                       PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString());
        string tilesFilePath = Path.Combine(localWorldPath, "Tiles.json");
        Directory.CreateDirectory(localWorldPath);
        TileInfo[,] tiles = ConvertNoiseValuesToTileInfos(noiseValues);
        if (tiles.Length < 1)
        {
            throw new Exception("No tiles generated.");
        }
        List<TileInfo> newTiles = new List<TileInfo>();
        for (int i = 0; i < tiles.GetLength(1); i++)
        {
            for (int j = 0; j < tiles.GetLength(0); j++)
            {
                newTiles.Add(tiles[j, i]);
            }
        }
        TileDataWrapper tileDataWrapper = new TileDataWrapper();
        tileDataWrapper.Tiles = newTiles.ToArray();
        string json = JsonUtility.ToJson(tileDataWrapper);
        File.WriteAllText(tilesFilePath, json);
    }

    TileInfo[,] ConvertNoiseValuesToTileInfos(float[,] noiseValues)
    {
        Debug.Log("Converting noise values to tile infos...");
        TileInfo[,] tiles = new TileInfo[noiseValues.GetLength(0), noiseValues.GetLength(1)];
        int id_counter = 0;
        for (int y = 0; y < noiseValues.GetLength(1); y++)
        {
            for (int x = 0; x < noiseValues.GetLength(0); x++)
            {
                GameObject tile = GetTileThroughNoiseValue(noiseValues[x, y]);
                tiles[x, y] = new TileInfo
                {
                    Type = tile.GetComponent<Tile>().TileInfo.Type,
                    BuildabilityPercentage = tile.GetComponent<Tile>().TileInfo.BuildabilityPercentage,
                    MovementModifierPercentage = tile.GetComponent<Tile>().TileInfo.MovementModifierPercentage,
                    PositionX = x,
                    PositionY = y,
                    ID = id_counter
                };
                Debug.Log("Created: " + tiles[x, y].Type + " with ID " + tiles[x, y].ID + " and positionX " + tiles[x, y].PositionX + "and positionY " + tiles[x, y].PositionY);
                id_counter++;
            }
        }
        for (int y = 0;y < noiseValues.GetLength(1); y++)
        {
            for (int x = 0; x < noiseValues.GetLength(0); x++)
            {
                Debug.Log("Tiles in tiles: " + tiles[x, y].Type + " with ID " + tiles[x, y].ID + " and positionX " + tiles[x, y].PositionX + "and positionY " + tiles[x, y].PositionY);
            }
        }
        return tiles;
    }

    GameObject GetTileThroughNoiseValue(float noiseValue)
    {
        Debug.Log("Converting noise value " + noiseValue + " to tile...");
        if (noiseValue < 15f)
        {
            return loadedAssets.DEEP_WATER_TILE_PREFAB;
        }
        if (noiseValue < 30f)
        {
            return loadedAssets.SHALLOW_WATER_TILE_PREFAB;
        }
        if (noiseValue < 45f)
        {
            return loadedAssets.SAND_TILE_PREFAB;
        }
        if (noiseValue < 60f)
        {
            return loadedAssets.DARK_DIRT_TILE_PREFAB;
        }
        return loadedAssets.BLANK_TILE_PREFAB;
    }

    float[,] GenerateLocalMapNoiseValues(float scale)
    {
        Debug.Log("Generating noise...");
        float[,] noiseValues = Noise.Calc2D(PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX, PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY, scale);
        return noiseValues;
    }

    bool CheckIfGenerationNeeded()
    {
        Debug.Log("Checking if generation needed...");
        if (CheckIfLocalWorldFolderExists())
        {
            if (CheckIfFileExists("Tiles.json"))
            {
                return false;
            }
        }
        return true;
    }

    bool CheckIfLocalWorldFolderExists()
    {
        Debug.Log("Checking if local world folder exists...");
        string localWorldPath = Path.Combine(
            Application.dataPath, 
            "World/Instances", 
            PersistentSessionInformation.Instance.loadedGlobalWorld.Key.ToString(), 
            PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString());
        if (Directory.Exists(localWorldPath))
        {
            Debug.Log("Local world folder exists.");
            return true;
        }
        Debug.Log("Local world folder does not exist.");
        return false;
    }

    bool CheckIfFileExists(string FileNameAndExtension) {
        Debug.Log("Checking if file " + FileNameAndExtension + " exists...");
        string tilesFilePath = Path.Combine(
            Application.dataPath, 
            "World/Instances", 
            PersistentSessionInformation.Instance.loadedGlobalWorld.Key.ToString(), 
            PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString(), 
            FileNameAndExtension);
        if (File.Exists(tilesFilePath))
        {
            Debug.Log("File " + FileNameAndExtension + " exists.");
            return true;
        }
        else
        {
            Debug.Log("File " + FileNameAndExtension + " does not exist.");
            return false;
        }
    }

    [Serializable]
    public class TileDataWrapper
    {
        public TileInfo[] Tiles;
    }
}
