using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To create a tile:
// 1. Add new enum
// 2. Prefab with different Tile Info [DONT FORGET TO MAKE ADDRESSABLE]
// 3. Add to LoadedAssetsScriptableObject
// 4. Add to AssetManager - GetTile
// 5. Add to LoadedAssetManager - LoadTileToMemory
// 6. Add to LoadedAssetManager - LoadTileToMemoryIfNeeded
// 7. Add to LocalWorldBuilderHelper - GetTileThroughNoiseValue


public class Tile : MonoBehaviour
{
    public TileInfo TileInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
