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

    public HexResourceData HexResourceData
    {
        get
        {
            return hexResourceData;
        }

        set
        {
            hexResourceData = value;
            if (ResourceController.Instance.OnHexResourceTypeChange != null)
                ResourceController.Instance.OnHexResourceTypeChange.Invoke(this);
        }
    }

    private HexResourceData hexResourceData;

    public Hex(Vector3 position)
    {
        Position = position;
        HexResourceData = null;
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
}