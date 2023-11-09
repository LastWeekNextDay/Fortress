using SimplexNoise;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalWorldBuilderHelper
{
    public static float[,] GenerateLocalMapNoiseValues(float scale)
    {
        Debug.Log("Generating noise...");
        float[,] noiseValues = Noise.Calc2D(PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeX, PersistentSessionInformation.Instance.loadedLocalWorld.Value.SizeY, scale);
        return noiseValues;
    }

    public static bool CheckIfLocalWorldFolderExists()
    {
        Debug.Log("Checking if local world folder exists...");
        string localWorldPath = Path.Combine(
            Application.dataPath,
            "World/Instances",
            PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString());
        if (Directory.Exists(localWorldPath))
        {
            Debug.Log("Local world folder exists.");
            return true;
        }
        Debug.Log("Local world folder does not exist.");
        return false;
    }

    public static bool CheckIfFileExists(string FileNameAndExtension)
    {
        Debug.Log("Checking if file " + FileNameAndExtension + " exists...");
        string tilesFilePath = Path.Combine(
            Application.dataPath,
            "World/Instances",
            PersistentSessionInformation.Instance.loadedLocalWorld.Key.ToString(),
            FileNameAndExtension);
        if (File.Exists(tilesFilePath))
        {
            Debug.Log("File " + FileNameAndExtension + " exists.");
            return true;
        }
        else
        {
            Debug.Log("File " + FileNameAndExtension + " does not exist.");
            return false;
        }
    }
}
