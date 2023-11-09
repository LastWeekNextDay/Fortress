using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSessionInformation : MonoBehaviour
{
    public static PersistentSessionInformation Instance;
    public int MaxMapSizeX = 250;
    public int MaxMapSizeY = 250;
    public KeyValuePair<int, LocalWorldInfo> loadedLocalWorld;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.Log("PersistentSessionInformation already exists!");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
