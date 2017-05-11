/* HexMapController.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private Vector2 Padding = Vector2.one * 0.05f;
    [SerializeField]
    private HexType hexType;
    [SerializeField] private Material material;


    [Header("Hex Mesh Properties")]
    [SerializeField]
    private float hexHeight = 0.3f;

    [Header("Collision")]
    public GameObject collisionQuad;

    [Header("Update Properties")]
    [SerializeField]
    private bool updateEveryFrame;

    public BidirectionalDictionary<Hex, GameObject> hexGameObjectMap { get; protected set; }
    public World World { get; protected set; }


    #region Unity Methods

    private void Awake()
    {
        hexGameObjectMap = new BidirectionalDictionary<Hex, GameObject>();
        ResourceController.Instance.OnHexResourceTypeChange += OnHexResourceTypeChanged;
        CreateWorld();

        collisionQuad.transform.localScale = new Vector3(mapSize * 4 * (hexSize.x + Padding.x), mapSize * 4 * (hexSize.y + Padding.y), 1);

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

    #endregion
    #region Creating World

    public HexMapLayout Layout { get; protected set; }

    /// <summary>
    /// Creates a grid of hexes based on the values set above
    /// </summary>
    private void CreateWorld()
    {
        World = new World();
        hexGameObjectMap.Clear();
        Layout = new HexMapLayout((hexType == HexType.Flat) ? HexOrientation.FlatTopped : HexOrientation.PointyTopped, hexSize, Vector2.zero);
        for (int q = -mapSize; q <= mapSize; q++)
        {
            int r1 = Mathf.Max(-mapSize, -q - mapSize);
            int r2 = Mathf.Min(mapSize, -q + mapSize);
            for (int r = r1; r <= r2; r++)
            {
                Hex h = new Hex(new HexCoord(q, r), World);
                SaveInteresctions(h, Layout);
                World.AddHex(h);
                GameObject obj = CreateHexGameObject(h, Layout);
                
                hexGameObjectMap.Add(h, obj);
            }
        }

        // FIXME: This is ungodly
        GameObject groundGO =
            CreateHexGameObject(
                new Hex(
                    new HexCoord(0, 0, 0),
                    World),
                new HexMapLayout(
                    (hexType != HexType.Flat) ? HexOrientation.FlatTopped : HexOrientation.PointyTopped,
                    hexSize * mapSize * Mathf.Sqrt(3) + hexSize * Mathf.Sqrt(3),
                    Vector2.zero));

        groundGO.transform.position = new Vector3(0, GroundHeight, 0);
        StaticBatchingUtility.Combine(this.transform.gameObject);
    }

    public float GroundHeight = -2f;

    /// <summary>
    /// HACK!!!!! I'm rounding to the nearest half to make this work!
    /// </summary>
    private class IntersectionCompaerer : IEqualityComparer<Vector3>
    {
        bool IEqualityComparer<Vector3>.Equals(Vector3 x, Vector3 y)
        {
            return x.RoundToNearestHalf() == y.RoundToNearestHalf();
        }

        int IEqualityComparer<Vector3>.GetHashCode(Vector3 a)
        {
            return a.RoundToNearestHalf().GetHashCode();
        }

    }

    protected Dictionary<Vector3, HexCorner> Corners = new Dictionary<Vector3, HexCorner>(new IntersectionCompaerer());

    private void SaveInteresctions(Hex h, HexMapLayout layout)
    {
        HexCorner[] hexCorners = new HexCorner[6];
        Vector3[] corners =
            CalculateHexCornerOffsets(layout, Vector2.zero)
            .Select<Vector2, Vector3>
            (
                (offset) =>
                {
                    Vector3 center = h.HexCoord.CalculateWorldPosition(layout);
                    return new Vector3(offset.x + center.x, 0, offset.y + center.z);
                }
            )
           .ToArray();
        for (int i = 0; i < 6; i++)
        {
            if (Corners.ContainsKey(corners[i]))
            {
                hexCorners[i] = Corners[corners[i]];
                hexCorners[i].AddNeighbor(h);
                //Debug.Log("Already in Dictionary");
            }
            else
            {
                hexCorners[i] = new HexCorner(corners[i]);
                Corners.Add(corners[i], hexCorners[i]);
                hexCorners[i].AddNeighbor(h);
            }
        }
        h.Corners = hexCorners;
    }


    private GameObject CreateHexGameObject(Hex h, HexMapLayout layout)
    {
        GameObject obj = new GameObject("Hex " + h.HexCoord.q + "_" + h.HexCoord.r + "_" + h.HexCoord.s);
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mr.material = material;
        MeshFilter mf = obj.AddComponent<MeshFilter>();



        // TODO: Extract this to a separate MeshData Class!
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        Vector2[] hexCorners = CalculateHexCornerOffsets(layout, Padding);

        // Calculate the top base of the hexagonal prisim

        #region Top Face

        Vector3[] cornersTop =
            hexCorners
            .Select(
                (corner) =>
                {
                    return new Vector3(corner.x, 0, corner.y);
                }
            )
            .ToArray();

        verts.AddRange(cornersTop);
        verts.Add(Vector3.zero);

        for (int i = 0; i < 6; i++)
        {
            tris.Add((i + 1) % 6);
            tris.Add(i);
            tris.Add(6);
        }

        #endregion

        #region Bottom Face

        int offsetAfterTopFace = verts.Count;

        Vector3[] cornersBottom =
            hexCorners
            .Select(
                (corner) =>
                {
                    return new Vector3(corner.x, -hexHeight, corner.y);
                }
            )
            .ToArray();

        verts.AddRange(cornersBottom);
        verts.Add(new Vector3(0, -hexHeight, 0));

        for (int i = 0; i < 6; i++)
        {
            tris.Add(i + offsetAfterTopFace);
            tris.Add((i + 1) % 6 + offsetAfterTopFace);
            tris.Add(6 + offsetAfterTopFace);
        }

        #endregion

        #region Lateral Faces

        int offsetAfterBottomFace = verts.Count;

        List<Vector3> topPlusBottomFaces = verts;
        verts.AddRange(topPlusBottomFaces);

        int offsetAfterFirstLateralFaces = verts.Count;

        verts.AddRange(topPlusBottomFaces);

        int offset;

        for (int i = 0; i < 5; i++)
        {
            offset = (i % 2 == 0) ? offsetAfterBottomFace : offsetAfterFirstLateralFaces;
            tris.AddRange(new int[] { i + 0 + offset, i + 1 + offset, i + 7 + offset });
            tris.AddRange(new int[] { i + 1 + offset, i + 8 + offset, i + 7 + offset });

        }

        // HACK because the modulo operator is wierd
        offset = offsetAfterFirstLateralFaces;
        tris.AddRange(new int[] { 5 + offset, 0 + offset, 12 + offset });
        tris.AddRange(new int[] { 0 + offset, 7 + offset, 12 + offset });

        #endregion

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        mf.mesh = mesh;

        obj.transform.position = h.HexCoord.CalculateWorldPosition(layout);
        obj.transform.SetParent(this.transform);
        obj.isStatic = true;
        
        return obj;
    }

    private Vector2 CalculateHexCornerOffset(HexMapLayout layout, int corner, Vector2 padding)
    {
        Vector2 size = layout.HexSize - padding;
        float angle = 2 * Mathf.PI * (layout.Orientation.start_angle + corner) / 6;
        return new Vector2(size.x * Mathf.Cos(angle), size.y * Mathf.Sin(angle));
    }

    private Vector2[] CalculateHexCornerOffsets(HexMapLayout layout, Vector2 padding)
    {
        Vector2[] corners = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            Vector2 offset = CalculateHexCornerOffset(layout, i, padding);
            corners[i] = new Vector2(offset.x, offset.y);
        }
        return corners;
    }

    #endregion

    #region Resources

    /// <summary>
    /// Callback for the ResourceController.Instance.OnHexResourceTypeChange Action.
    /// </summary>
    /// <param name="h">The hex</param>
    public void OnHexResourceTypeChanged(Hex h)
    {
        GameObject obj = hexGameObjectMap[h];
        obj.GetComponent<MeshRenderer>().sharedMaterial = h.HexResourceData.material;
    }
    #endregion


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
        return World.GetHex(q, r, s);
    }

    public Hex GetHex(HexCoord coord)
    {
        return World.GetHex(coord);
    }
    public bool InRange(HexCoord coord)
    {
        return Mathf.Abs(coord.q) <= mapSize && Mathf.Abs(coord.r) <= mapSize && Mathf.Abs(coord.s) <= mapSize;
    }
    #endregion
}