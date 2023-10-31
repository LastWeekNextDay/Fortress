using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalWorld : MonoBehaviour
{
    public int TileCountX;
    public int TileCountY;
    public GameObject[,] Tiles;
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
        TileCountX = sizeX;
        TileCountY = sizeY;
        Tiles = new GameObject[TileCountX, TileCountY];
    }
}
