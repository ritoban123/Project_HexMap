/* HexMapDisplay.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapDisplay : MonoBehaviour
{
    private static HexMapDisplay _instance;
    public static HexMapDisplay Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<HexMapDisplay>();
            return _instance;
        }
    }


    [SerializeField] private GameObject hexPrefab;

    private void Start()
    {
        ResourceController.Instance.OnHexResourceTypeChange +=
            (hex) =>
            {
                OnResourceTypeChange(hex, HexMapController.Instance.GetGameObjectForHex(hex));
            };
    }


    public GameObject CreateHex(Hex hex, int i, int j)
    {
        GameObject hexGO = Instantiate(
                    hexPrefab,
                    hex.Position,
                    Quaternion.identity, this.transform
                    );

        hexGO.name = hexPrefab.name + " " + i + " " + j;
        return hexGO;
    }

    public void OnResourceTypeChange(Hex hex, GameObject go)
    {
        go.GetComponentInChildren<MeshRenderer>().material.color = hex.HexResourceData.color;
    }
}
