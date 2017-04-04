/* HexGrid.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class HexMapController : MonoBehaviour
{

    private static HexMapController _instance;
    public static HexMapController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<HexMapController>();
            return _instance;
        }
    }

    public int Width
    {
        get
        {
            return width;
        }
    }
    public int Height
    {
        get
        {
            return height;
        }
    }

    public Hex[] AllHexes
    {
        get
        {
            return hexGameObjectMap.Keys.ToArray<Hex>();
        }
    }

    public Hex[,] HexMap { get; protected set; }


    [Header("Hex Grid Properties")]


    [SerializeField]
    private int width = 10;
    [SerializeField] private int height = 10;

    [Header("Hex Properties")]

    [SerializeField]
    private float hexDiameter = 2f;
    [SerializeField] private float zScale = 0.83f;
    [SerializeField] private float xScale = 0.93f;

    [Header("Camera Offset Values")]

    [SerializeField]
    private float cameraXOffset = 0;
    [SerializeField] private float cameraZOffset = -20;

    // DO NOT USE!! THESE WILL CAUSE ERRORS WHILE INTERACTING WITH OTHER SCRIPTS
    //[Header("Update Values")]
    //[SerializeField]
    //private bool updateHexMapEveryFrame = false;
    //[SerializeField] private bool updateCameraEveryFrame = false;


    public BidirectionalDictionary<Hex, GameObject> hexGameObjectMap { get; protected set; }


    private void Awake()
    {
        hexGameObjectMap = new BidirectionalDictionary<Hex, GameObject>();
        // TODO: Add padding to the width and the height for generating the graph
        HexMap = new Hex[width, height]; 
        // HACK to ensure that this runs before the Resource Controller, which relies on this
        CreateHexGrid();
        RecalculateMainCameraPosition();
    }

    //private void Update()
    //{
    //    if (updateHexMapEveryFrame)
    //    {
    //        foreach (Transform child in this.transform)
    //        {
    //            Destroy(child.gameObject);
    //        }
    //        CreateHexGrid();
    //    }
    //    if (updateCameraEveryFrame)
    //    {
    //        RecalculateMainCameraPosition();
    //    }
    //}

    /// <summary>
    /// Creates a grid of hexes based on the values set above
    /// </summary>
    private void CreateHexGrid()
    {
        // FIXME: Use cube coordinates!
        for (int j = 0; j < height ; j++)
        {
            for (int i = 0; i < width; i++)
            {
                Vector3 pos = new Vector3(
                     i * hexDiameter * xScale + ((j % 2 == 1) ? hexDiameter * xScale / 2 : 0),
                     0,
                     j * hexDiameter * zScale);
                Hex hex = new Hex(pos, i, j);
                GameObject hexGo = HexMapDisplay.Instance.CreateHex(hex, i, j);
                hexGameObjectMap.Add(hex, hexGo);
                HexMap[i, j] = hex;
            }
        }
    }


    // FIXME: This should be on a camera manager, not the hex map controller
    private void RecalculateMainCameraPosition()
    {
        Transform camTrans = Camera.main.transform;
        if (camTrans == null)
            camTrans = GameObject.FindObjectOfType<Camera>().transform;
        if (camTrans == null)
        {
            Debug.LogError("HexGrid::RecalculateMainCameraPosition - Could not find camera");
            return;
        }

        camTrans.position = new Vector3(
            width * hexDiameter * xScale / 2 + cameraXOffset,
            camTrans.position.y,
            height * hexDiameter * zScale / 2 + cameraZOffset);
    }

    #region Utility Methods
    public GameObject GetGameObjectForHex(Hex hex)
    {
        if (hexGameObjectMap.ContainsKey(hex) == false)
            return null; // ALERT: This does not throw an error
        return hexGameObjectMap[hex];
    }

    public Hex GetHexForGameObject(GameObject obj)
    {
        if (hexGameObjectMap.ContainsValue(obj) == false)
            return null; // ALERT: This does not throw an error
        return hexGameObjectMap[obj];
    }

    public Hex GetHex(int x, int y)
    {
        if (x >= width || x < 0 || y >= height || y < 0)
            return null;
        return HexMap[x, y];
    }
    #endregion
}