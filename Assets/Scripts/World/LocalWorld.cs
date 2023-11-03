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

    public GameObject FindTileByID(int id)
    {
        Debug.Log("Finding tile with ID " + id);
        for (int x = 0; x < TileCountX; x++)
        {
            for (int y = 0; y < TileCountY; y++)
            {
                if (Tiles[x, y].GetComponent<Tile>().TileInfo.ID == id)
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
