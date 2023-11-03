using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSessionInformation : MonoBehaviour
{
    public static PersistentSessionInformation Instance;
    public int MaxMapSizeX = 100;
    public int MaxMapSizeY = 100;
    public KeyValuePair<int, GlobalWorldInfo> loadedGlobalWorld;
    public KeyValuePair<int, LocalWorldInfo> loadedLocalWorld;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("PersistentSessionInformation already exists!");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
