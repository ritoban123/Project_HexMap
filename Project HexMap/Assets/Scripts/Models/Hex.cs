using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Hex 
{
    public HexCoord HexCoord { get; protected set; }
    public HexCorner[] Corners { get; set; }
    public World World;
    

    public Hex(HexCoord hexCoord, World world)
    {
        HexCoord = hexCoord;
        World = world;
    }
    
    #region Resource 
    private HexResourceData hexResourceData;

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



    public void UpdateResources(Random rand)
    {
        if (hexResourceData == null)
            return;
        for (int i = 0; i < hexResourceData.HexResourcesPerMonth.Length; i++)
        {
            HexResourceData.HexResource resourceData = HexResourceData.HexResourcesPerMonth[i];
            float prob = (float)rand.NextDouble();
            if(prob <= resourceData.CollectProbability)
                ResourceController.Instance.AddResource(resourceData.Resource, resourceData.AmountPerMonth);
            else
            {
                // Consider giving the user a fraction of what they would usually get?
                continue;
            }
        }
    }

    #endregion
}
