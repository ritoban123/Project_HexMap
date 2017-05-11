/* HexResourceData.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hex Resource Data", menuName = "Custom/Hex Resource Type", order = -1)]
public class HexResourceData : ScriptableObject 
{
    [Serializable]
    public struct HexResource
    {
        public string Resource;
        public float AmountPerMonth;
        public float CollectProbability;

    }

    public HexResource[] HexResourcesPerMonth = new HexResource[2];
    public Material material;
}
