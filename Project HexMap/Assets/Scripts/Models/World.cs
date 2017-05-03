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
    public Dictionary<HexCorner, Settlement> Settlements { get; protected set; }
    public Dictionary<String, float> Resources { get; protected set; }


    public World()
    {
        HexMap = new Dictionary<HexCoord, Hex>();
        Settlements = new Dictionary<HexCorner, Settlement>();
        Resources = new Dictionary<string, float>();

    }

    #region Dictionary Helper Methods

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
        if (HexMap.ContainsKey(coord) == false)
        {
            throw new ArgumentException(coord.ToString() + " was not found in HexMap");
        }
        return HexMap[coord];
    }


    public void AddSettlement(Settlement s)
    {
        Settlements.Add(s.HexCorner, s);
    }

    public void RemoveSettlement(Settlement s)
    {
        Settlements.Remove(s.HexCorner);
    }


    public Settlement GetSettlement(HexCorner corner)
    {
        if (Settlements.ContainsKey(corner) == false)
        {
            throw new ArgumentException(corner.ToString() + " was not found in HexMap");
        }
        return Settlements[corner];
    }

    #endregion Dictionary Helper Methods

    #region Action Helpers
    public void ForEachSettlment(Action<Settlement> a)
    {
        foreach (Settlement s in Settlements.Values)
        {
            a.Invoke(s);
        }
    }
    #endregion
}
