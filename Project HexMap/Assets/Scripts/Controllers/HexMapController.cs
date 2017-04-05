/* HexMapController.cs  
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

    public enum HexType { Flat, Pointy }


    [Header("Hex Grid Properties")]
    [SerializeField]
    private int mapSize = 10;
    [SerializeField]
    private Vector2 hexSize = Vector2.one;
    [SerializeField]
    private Vector2 padding = Vector2.one * 0.05f;
    [SerializeField]
    private HexType hexType;

    [Header("Update Properties")]
    private bool updateEveryFrame;

    public BidirectionalDictionary<Hex, GameObject> hexGameObjectMap { get; protected set; }


    private void Awake()
    {
        hexGameObjectMap = new BidirectionalDictionary<Hex, GameObject>();
        // HACK to ensure that this runs before the Resource Controller, which relies on this
        CreateWorld();
    }

    private void Update()
    {
        if (updateEveryFrame)
        {
            foreach (Transform child in transform)
            {
                child.SetParent(null);
                Destroy(child.gameObject);
            }
            CreateWorld();
        }
    }

    public World World { get; protected set; }

    /// <summary>
    /// Creates a grid of hexes based on the values set above
    /// </summary>
    private void CreateWorld()
    {
        World = new World();
        HexMapLayout layout = new HexMapLayout((hexType == HexType.Flat) ? HexOrientation.FlatTopped : HexOrientation.PointyTopped, hexSize, Vector2.zero);
        for (int q = -mapSize; q <= mapSize; q++)
        {
            int r1 = Mathf.Max(-mapSize, -q - mapSize);
            int r2 = Mathf.Min(mapSize, -q + mapSize);
            for (int r = r1; r <= r2; r++)
            {
                Hex h = new Hex(new HexCoord(q, r), null);
                World.AddHex(h);
                GameObject obj = CreateHexGameObject(h, layout);
                hexGameObjectMap.Add(h, obj);  
            }
        }

    }

    private GameObject CreateHexGameObject(Hex h, HexMapLayout layout)
    {
        GameObject obj = new GameObject("Hex " + h.HexCoord.q + "_" + h.HexCoord.r + "_" + h.HexCoord.s);
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector3[] corners =
            CalculateHexCorners(layout, h, padding)
            .Select(
                (corner) =>
                {
                    return new Vector3(corner.x, 0, corner.y);
                }
            )
            .ToArray();

        // TODO: Extract this to a separate MeshData Class!
        List<Vector3> verts = new List<Vector3>(corners);
        verts.Add(Vector3.zero);

        List<int> tris = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            tris.Add((i + 1) % 6);
            tris.Add(i);
            tris.Add(6);
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        mf.mesh = mesh;

        obj.transform.position = h.HexCoord.CalculateWorldPosition(layout);
        obj.transform.SetParent(this.transform);

        return obj;
    }

    private Vector2 CalculateHexCornerOffset(HexMapLayout layout, int corner, Vector2 padding)
    {
        Vector2 size = layout.HexSize - padding;
        float angle = 2 * Mathf.PI * (layout.Orientation.start_angle + corner) / 6;
        return new Vector2(size.x * Mathf.Cos(angle), size.y * Mathf.Sin(angle));
    }

    private Vector2[] CalculateHexCorners(HexMapLayout layout, Hex h, Vector2 padding)
    {
        Vector2[] corners = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            Vector2 offset = CalculateHexCornerOffset(layout, i, padding);
            corners[i] = new Vector2(offset.x, offset.y);
        }
        return corners;
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

    public Hex GetHex(int q, int r, int s)
    {
        return null;
    }
    #endregion
}