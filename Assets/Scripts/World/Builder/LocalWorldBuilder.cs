using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
using UnityEngine.AddressableAssets;
using System.IO;

public class LocalWorldBuilder : MonoBehaviour
{
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
        TileBuilder.FillWorldWithTiles();
    }
}
