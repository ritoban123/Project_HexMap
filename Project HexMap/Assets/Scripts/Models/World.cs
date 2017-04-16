/* World.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public Dictionary<HexCoord, Hex> HexMap { get; protected set; }
    
    public World()
    {
        HexMap = new Dictionary<HexCoord, Hex>();
    }

    public void AddHex(Hex hex)
    {
        HexMap.Add(hex.HexCoord, hex);
    }

    public void RemoveHex(Hex hex)
    {
        HexMap.Remove(hex.HexCoord);
    }

    public Hex GetHex(int q, int r, int s)
    {
        return HexMap[new HexCoord(q, r, s)];
    }

    public Hex GetHex(HexCoord coord)
    {
        if(HexMap.ContainsKey(coord) == false)
        {
            throw new ArgumentException(coord.ToString() + " was not found in HexMap");
        }
        return HexMap[coord];
    }
}
