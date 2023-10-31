using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalWorld : MonoBehaviour
{
    public static LocalWorld Instance;

    public int TileCountX;
    public int TileCountY;
    public GameObject[,] Tiles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("LocalWorld already exists!");
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

    public void Initialize(int sizeX, int sizeY)
    {
        Debug.Log("Initializing Local World...");
        // Next time implemented using LocalWorldInfo instead of sizeX and sizeY
        TileCountX = sizeX;
        TileCountY = sizeY;
        Tiles = new GameObject[TileCountX, TileCountY];
        FillTiles();
    }

    void FillTiles()
    {
        Debug.Log("Filling tiles...");
        int id_counter = 0;
        for (int x = 0; x < TileCountX; x++)
        {
            for (int y = 0; y < TileCountY; y++)
            {
                Tiles[x, y] = AssetManager.Instance.Spawn(AssetManager.Instance.LoadTile(TileType.BLANK), new Vector3(x, y, 0));
                Tiles[x, y].GetComponent<Tile>().ID = id_counter;
                id_counter++;
            }
        }
    }

    public GameObject FindTileByID(int id)
    {
        Debug.Log("Finding tile with ID " + id);
        for (int x = 0; x < TileCountX; x++)
        {
            for (int y = 0; y < TileCountY; y++)
            {
                if (Tiles[x, y].GetComponent<Tile>().ID == id)
                {
                    Debug.Log("Found tile with ID " + id);
                    return Tiles[x, y];
                }
            }
        }
        Debug.LogError("Could not find tile with ID " + id);
        return null;
    }
}
