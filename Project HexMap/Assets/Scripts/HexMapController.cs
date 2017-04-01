/* HexGrid.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapController : MonoBehaviour
{

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

    [Header("Hex Grid Properties")]

    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    [Header("Hex Properties")]

    [SerializeField] private float hexDiameter = 2f;
    [SerializeField] private float zScale = 0.83f;
    [SerializeField] private float xScale = 0.93f;

    [Header("Camera Offset Values")]

    [SerializeField] private float cameraXOffset = 0;
    [SerializeField] private float cameraZOffset = -20;


    [Header("Update Values")]
    [SerializeField] private bool updateHexMapEveryFrame = false;
    [SerializeField] private bool updateCameraEveryFrame = false;


    private void Start()
    {
        CreateHexGrid();
        RecalculateMainCameraPosition();
    }

    private void Update()
    {
        if(updateHexMapEveryFrame)
        {
            foreach(Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            CreateHexGrid();
        }
        if (updateCameraEveryFrame)
        {
            RecalculateMainCameraPosition();
        }
    }

    /// <summary>
    /// Creates a grid of hexes based on the values set above
    /// </summary>
    private void CreateHexGrid()
    {
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                GameObject hexGO = Instantiate(
                    hexPrefab,
                    new Vector3(
                        i * hexDiameter * xScale + ((j % 2 == 1) ? hexDiameter * xScale / 2 : 0),
                        0,
                        j * hexDiameter * zScale),
                    Quaternion.identity, this.transform
                    );
                hexGO.name = hexPrefab.name + " " + i + " " + j;
            }
        }
    }

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
            width * hexDiameter * xScale/ 2 + cameraXOffset, 
            camTrans.position.y,
            height * hexDiameter * zScale/ 2 + cameraZOffset);

    }
}