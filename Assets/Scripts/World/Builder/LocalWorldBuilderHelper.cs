using SimplexNoise;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalWorldBuilderHelper
{
    public static float[,] GenerateLocalMapNoiseValues(float scale)
    {
        Debug.Log("Generating noise...");
        float[,] noiseValues = Noise.Calc2D(PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX, PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY, scale);
        return noiseValues;
    }

    public static bool CheckIfGenerationNeeded()
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

    public static bool CheckIfLocalWorldFolderExists()
    {
        Debug.Log("Checking if local world folder exists...");
        string localWorldPath = Path.Combine(
            Application.dataPath,
            "World/Instances",
            PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString());
        if (Directory.Exists(localWorldPath))
        {
            Debug.Log("Local world folder exists.");
            return true;
        }
        Debug.Log("Local world folder does not exist.");
        return false;
    }

    public static bool CheckIfFileExists(string FileNameAndExtension)
    {
        Debug.Log("Checking if file " + FileNameAndExtension + " exists...");
        string tilesFilePath = Path.Combine(
            Application.dataPath,
            "World/Instances",
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

    public static GameObject GetTileThroughNoiseValue(float noiseValue)
    {
        Debug.Log("Converting noise value " + noiseValue + " to tile...");
        AssetManager assetManager = AssetManager.Instance;
        if (noiseValue < 20f)
        {
            return assetManager.GetTile(TileType.DEEP_WATER);
        }
        if (noiseValue < 40f)
        {
            return assetManager.GetTile(TileType.SHALLOW_WATER);
        }
        if (noiseValue < 70f)
        {
            return assetManager.GetTile(TileType.SAND);
        }
        if (noiseValue < 100f)
        {
            return assetManager.GetTile(TileType.DIRT_DARK);
        }
        if (noiseValue < 180)
        {
            return assetManager.GetTile(TileType.GRASS);
        }
        if (noiseValue < 250)
        {
            return assetManager.GetTile(TileType.STONE);
        }
        return assetManager.GetTile(TileType.BLANK);
    }

    public static GameObject[,] LoadTilesFromFile()
    {
        Debug.Log("Loading tiles...");
        string tilesFilePath = Path.Combine(
                       Application.dataPath,
                       "World/Instances",
                       PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString(),
                       "Tiles.json");
        string json = File.ReadAllText(tilesFilePath);
        TileDataWrapper tileDataWrapper = JsonUtility.FromJson<TileDataWrapper>(json);
        TileInfo[] tiles1D = tileDataWrapper.Tiles;
        Debug.Log("Tiles loaded: " + tiles1D);
        TileInfo[,] tiles2D = Convert1DTilesInfoArrayTo2D(tiles1D);
        return ConvertTilesInfosIntoTilesGameObjects(tiles2D);
    }

    public static void SaveTilesToFile(TileInfo[,] tilesInfo)
    {
        string localWorldPath = Path.Combine(
                       Application.dataPath,
                       "World/Instances",
                       PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString());
        string tilesFilePath = Path.Combine(localWorldPath, "Tiles.json");
        Directory.CreateDirectory(localWorldPath);
        TileDataWrapper tileDataWrapper = new TileDataWrapper();
        tileDataWrapper.Tiles = MiscHelper.ConvertTo1DArray(tilesInfo);
        string json = JsonUtility.ToJson(tileDataWrapper);
        File.WriteAllText(tilesFilePath, json);
    }

    public static TileInfo GetTileInfoOfCoordinates(int x, int y, TileInfo[,] tiles)
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

    public static TileInfo[,] ConvertNoiseValuesToTileInfos(float[,] noiseValues)
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
        return tiles;
    }

    public static TileInfo[,] GetTileInfoFromTiles(GameObject[,] tiles)
    {
        TileInfo[,] tilesInfo = new TileInfo[tiles.GetLength(0), tiles.GetLength(1)];
        for (int y = 0; y <= tiles.GetLength(0); y++)
        {
            for(int x = 0; x <= tiles.GetLength(1); x++)
            {
                tilesInfo[x, y] = tiles[x, y].GetComponent<Tile>().TileInfo;
            }
        }
        return tilesInfo;
    }

    public static TileInfo[,] Convert1DTilesInfoArrayTo2D(TileInfo[] tiles) {
        TileInfo[,] tiles2D = new TileInfo[PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX,
                                                PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY];
        for (int y = 0; y < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY; y++)
        {
            for (int x = 0; x < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX; x++)
            {
                foreach (TileInfo tileInfo in tiles)
                {
                    if (tileInfo.PositionX == x && tileInfo.PositionY == y)
                    {
                        tiles2D[x, y] = tileInfo;
                    }
                }
            }
        }
        return tiles2D;
    }

    public static GameObject[,] ConvertTilesInfosIntoTilesGameObjects(TileInfo[,] tileInfos)
    {
        GameObject tilesObj = new GameObject("Tiles");
        tilesObj.transform.parent = GameObject.Find("LocalWorld").transform;
        GameObject[,] tilesGameObjects = new GameObject[PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX,
                                                        PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY];
        for (int y = 0; y < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY; y++)
        {
            for (int x = 0; x < PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX; x++)
            {
                TileInfo tileInfo = GetTileInfoOfCoordinates(x, y, tileInfos);
                if (tileInfo == null)
                {
                    continue;
                }
                AssetManager assetManager = GameObject.Find("AssetManager").GetComponent<AssetManager>();
                GameObject tile = assetManager.GetTile(tileInfo.Type);
                tilesGameObjects[x, y] = assetManager.SpawnObject(tile, new Vector3(x, y, 0), tilesObj.transform);
                tilesGameObjects[x, y].GetComponent<Tile>().TileInfo = tileInfo;
                tilesGameObjects[x, y].name = tileInfo.Type.ToString() + tileInfo.ID;
            }
        }
        return tilesGameObjects;
    }
}
