using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileBuilder
{
    public static void FillWorldWithTiles()
    {
        Debug.Log("Filling tiles...");
        LocalWorld localWorld = GameObject.Find("LocalWorld").GetComponent<LocalWorld>();
        if (TileBuilderHelper.CheckIfGenerationNeeded())
        {
            Debug.Log("Tiles not found. Generating map tiles...");
            GenerateTiles();
        }
        localWorld.Tiles = LoadTilesFromFile();
    }

    public static void GenerateTiles()
    {
        Debug.Log("Generating tiles...");
        float[,] noiseValues = LocalWorldBuilderHelper.GenerateLocalMapNoiseValues(0.005f);
        TileInfo[,] tileInfos = TileBuilderHelper.ConvertNoiseValuesToTileInfos(noiseValues);
        if (tileInfos.Length < 1)
        {
            throw new Exception("No tiles generated.");
        }
        SaveTilesToFile(tileInfos);
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
        TileInfo[,] tiles2D = TileBuilderHelper.Convert1DTilesInfoArrayTo2D(tiles1D);
        return TileBuilderHelper.ConvertTilesInfosIntoTilesGameObjects(tiles2D);
    }

    public static void SaveTilesToFile(TileInfo[,] tilesInfo)
    {
        Debug.Log("Saving tiles...");
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
}
