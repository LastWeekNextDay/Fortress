using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileInfo
{
    public int ID = -1;
    public TileType Type;
    public int PositionX;
    public int PositionY;
    public float BuildabilityPercentage;
    public float MovementModifierPercentage;
}

