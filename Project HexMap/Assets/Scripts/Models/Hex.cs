using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex 
{
    public HexCoord HexCoord { get; protected set; }
    

    public Hex(HexCoord hexCoord, HexResourceData hexResourceData)
    {
        HexCoord = hexCoord;
        this.hexResourceData = hexResourceData;
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



    public void UpdateResources()
    {
        if (hexResourceData == null)
            return;
        for (int i = 0; i < hexResourceData.HexResourcesPerMonth.Length; i++)
        {
            ResourceController.Instance.AddResource(
                hexResourceData.HexResourcesPerMonth[i].Resource,
                hexResourceData.HexResourcesPerMonth[i].AmountPerMonth
                );
        }
    }

    #endregion
}
