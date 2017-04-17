using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCorner
{
    public Vector3 WorldPosition { get; protected set; } 
    // TODO: Add in neighboring hexes

    public HexCorner(Vector3 pos)
    {
        WorldPosition = pos;
    }

    public static bool operator ==(HexCorner a, HexCorner b)
    {
        if(object.ReferenceEquals(a, null))
        {
            return object.ReferenceEquals(b, null);
        }
        if (object.ReferenceEquals(b, null))
        {
            return object.ReferenceEquals(a, null);
        }
        Vector3 diff = a.WorldPosition - b.WorldPosition;
        if (diff.sqrMagnitude <= 0.05)
            return true;
        else
            return false;
    }

    public static bool operator !=(HexCorner a, HexCorner b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is HexCorner)
            return (HexCorner)obj == this;
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return WorldPosition.GetHashCode();
    }
}
