/* Hex.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownController : MonoBehaviour
{
    protected Hex[,] HexMap;

    private void Start()
    {
        HexMap = HexMapController.Instance.HexMap;  
    }

    private void OnDrawGizmos()
    {
        if (HexMap == null)
            return;
        foreach (Hex hex in HexMap)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hex.Position, 0.3f);
            Hex[] neighbors = hex.GetNeighbors();
            foreach (Hex neighbor in neighbors)
            {
                if (neighbor != null)
                    Gizmos.DrawLine(new Vector3(hex.Position.x, 0, hex.Position.z), new Vector3(neighbor.Position.x, 0, neighbor.Position.z));
            }
        }
    }

}
