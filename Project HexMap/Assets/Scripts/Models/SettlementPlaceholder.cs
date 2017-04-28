using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementPlaceholder
{
    public HexCorner HexCorner;

    public SettlementPlaceholder(HexCorner hexCorner)
    {
        HexCorner = hexCorner;
    }

    public bool IsPositionValid(World world)
    {
        // Check if another existing settlement is on a hex corner that shares 2 neighbors with our hex corner
        if (world.Settlements.ContainsKey(HexCorner) == true)
            return false;

        if (world.Settlements.Count <= 0)
            return true; // We don't care where you place the first one

        bool adjacent = false;
        foreach (HexCorner hc in world.Settlements.Keys)
        {
            HashSet<Hex> common = new HashSet<Hex>(hc.Neighbors);
            common.IntersectWith(HexCorner.Neighbors);
            if(common.Count >= 2)
            {
                adjacent = true;
                break;
            }
        }


        return adjacent;

    }
}
