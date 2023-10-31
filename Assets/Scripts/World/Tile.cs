using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int _ID = -1;

    public float BuildabilityPercentage;
    public float MovementModifierPercentage;
    public int ID
    {
        get => _ID;
        set
        {
            if (_ID == -1)
            {
                _ID = value;
            }
            else
            {
                Debug.LogError("Cannot change ID of tile after it has been set");
            }
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
}
