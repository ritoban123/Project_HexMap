/* HexCoord.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a Cubic Hex Coordinate (q,r,s) along with functions for checking its equality and calculating distance
/// </summary>
public struct HexCoord
{

    public static readonly HexCoord[] HEX_NEIGHBOR_DIRECTIONS = new HexCoord[]{
    new HexCoord(1, 0, -1),new  HexCoord(1, -1, 0), new HexCoord(0, -1, 1),
    new HexCoord(-1, 0, 1), new HexCoord(-1, 1, 0), new HexCoord(0, 1, -1)
};

    public int q { get; private set; }
    public int r { get; private set; }
    public int s { get { return -q - r; } }


    public Vector3 CalculateWorldPosition(HexMapLayout layout)
    {
        HexOrientation M = layout.Orientation;
        float x = (M.f0 * q + M.f1 * r) * layout.HexSize.x;
        float y = (M.f2 * q + M.f3 * r) * layout.HexSize.y;
        return new Vector3(x + layout.Origin.x, 0, y + layout.Origin.y);
    }

    public static Vector3 WorldPositionToQRS(HexMapLayout layout, Vector3 p)
    {
        HexOrientation M = layout.Orientation;
        Vector2 pt = new Vector2((p.x - layout.Origin.x) / layout.HexSize.x,
                     (p.z - layout.Origin.y) / layout.HexSize.y);
        float q = M.b0 * pt.x + M.b1 * pt.y;
        float r = M.b2 * pt.x + M.b3 * pt.y;
        return new Vector3(q, r, -q - r);
    }

    #region Operators

    // NOTE: if 2 hexes have the same position, we are treating them as the same hex!

    public static bool operator ==(HexCoord a, HexCoord b)
    {
        return a.q == b.q && a.r == b.r && a.s == b.s;
    }

    public static bool operator !=(HexCoord a, HexCoord b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is HexCoord)
            return (HexCoord)obj == this;
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return 17 * q.GetHashCode() + r.GetHashCode();
    }

    private static HexCoord SubtractHexCoords(HexCoord a, HexCoord b)
    {
        return new HexCoord(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    private static HexCoord AddHexCoords(HexCoord a, HexCoord b)
    {
        return new HexCoord(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static int GetHexCoordLength(HexCoord hex)
    {
        return (int)((Mathf.Abs(hex.q) + Mathf.Abs(hex.r) + Mathf.Abs(hex.s)) / 2);
    }

    public static int GetHexDistance(HexCoord a, HexCoord b)
    {
        return GetHexCoordLength(SubtractHexCoords(a, b));
    }

    private HexCoord HexDirection(int direction /* 0 to 5 */)
    {
        direction = 6 + (direction % 6) % 6;
        return HEX_NEIGHBOR_DIRECTIONS[direction];
    }

    public HexCoord GetHexNeighbors(HexCoord hex, int direction)
    {
        return AddHexCoords(hex, HexDirection(direction));
    }

    #endregion

    #region Contructors

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public HexCoord(int q, int r, int s) : this(q, r)
    {
        if (q + r + s != 0)
            throw new ArgumentException("Hex::Hex - Q+R+S not equal to 0");
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        String result = "(" + q + ", " + r + ", " + s + ")";
        return result;
    }
    #endregion
}
