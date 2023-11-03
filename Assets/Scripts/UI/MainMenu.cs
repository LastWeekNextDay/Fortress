using SimplexNoise;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LocalWorldInfo localWorldInfo = new LocalWorldInfo();
        localWorldInfo.SizeX = 200;
        localWorldInfo.SizeY = 200;
        Noise.Seed = Random.Range(0, int.MaxValue);
        KeyValuePair<int, LocalWorldInfo> localWorld = new KeyValuePair<int, LocalWorldInfo>(0, localWorldInfo);
        PersistentSessionInformation.Instance.loadedLocalWorld = localWorld;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("LocalWorld");
    }
}
