using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileBuilderHelper
{
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

    public static bool CheckIfGenerationNeeded()
    {
        Debug.Log("Checking if generation needed...");
        if (LocalWorldBuilderHelper.CheckIfLocalWorldFolderExists())
        {
            if (LocalWorldBuilderHelper.CheckIfFileExists("Tiles.json"))
            {
                return false;
            }
        }
        return true;
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
            for (int x = 0; x <= tiles.GetLength(1); x++)
            {
                tilesInfo[x, y] = tiles[x, y].GetComponent<Tile>().TileInfo;
            }
        }
        return tilesInfo;
    }

    public static TileInfo[,] Convert1DTilesInfoArrayTo2D(TileInfo[] tiles)
    {
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
