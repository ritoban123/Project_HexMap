/* Hex.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    public Vector3 Position { get; protected set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public HexResourceData HexResourceData
    {
        get
        {
            return hexResourceData;
        }

        set
        {
            hexResourceData = value;
            if (ResourceController.Instance.OnHexResourceTypeChange != null && hexResourceData != null)
                ResourceController.Instance.OnHexResourceTypeChange.Invoke(this);
        }
    }

    private HexResourceData hexResourceData;

    public Hex(Vector3 position, int x, int y)
    {
        Position = position;
        X = x;
        Y = y;
    }

    public void UpdateResources()
    {
        for (int i = 0; i < hexResourceData.HexResourcesPerMonth.Length; i++)
        {
            ResourceController.Instance.AddResource(
                hexResourceData.HexResourcesPerMonth[i].Resource,
                hexResourceData.HexResourcesPerMonth[i].AmountPerMonth
                );
        }
    }

    public Hex[] GetNeighbors()
    {
        Hex[] neighbors = new Hex[6]; // We are assuming that they have 6 neighbors

        /*
         * var directions = [
           [ Hex(+1,  0), Hex( 0, -1), Hex(-1, -1),
             Hex(-1,  0), Hex(-1, +1), Hex( 0, +1) ],
           [ Hex(+1,  0), Hex(+1, -1), Hex( 0, -1),
             Hex(-1,  0), Hex( 0, +1), Hex(+1, +1) ]
        ]
        */

        if(Y % 2 == 0)
        {
            neighbors[0] = HexMapController.Instance.GetHex(X + 1, Y);
            neighbors[1] = HexMapController.Instance.GetHex(X, Y-1);
            neighbors[2] = HexMapController.Instance.GetHex(X - 1, Y - 1);
            neighbors[3] = HexMapController.Instance.GetHex(X - 1, Y);
            neighbors[4] = HexMapController.Instance.GetHex(X - 1, Y + 1);
            neighbors[5] = HexMapController.Instance.GetHex(X, Y + 1);
        }
        else if (Y % 2 == 1)
        {
            neighbors[0] = HexMapController.Instance.GetHex(X + 1, Y);
            neighbors[1] = HexMapController.Instance.GetHex(X +1, Y - 1);
            neighbors[2] = HexMapController.Instance.GetHex(X, Y - 1);
            neighbors[3] = HexMapController.Instance.GetHex(X - 1, Y);
            neighbors[4] = HexMapController.Instance.GetHex(X, Y + 1);
            neighbors[4] = HexMapController.Instance.GetHex(X + 1, Y + 1);
        }

        return neighbors;

    }
}
 