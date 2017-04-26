using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    public HexCorner HexCorner;

    public Settlement(SettlementPlaceholder placeholder)
    {
        HexCorner = placeholder.HexCorner;
    }
}
