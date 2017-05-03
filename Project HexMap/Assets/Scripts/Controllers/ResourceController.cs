/* ResourceController.cs  
(c) 2017 Ritoban Roy-Chowdhury. All rights reserved 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ResourceController : MonoBehaviour
{
    private static ResourceController _instance;
    public static ResourceController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ResourceController>();
            return _instance;
        }
    }

    public Action<Hex> OnHexResourceTypeChange = null;

    public World World
    {
        get
        {
            return HexMapController.Instance.World;
        }
    }

    [Header("Initialization of Resources")]
    [SerializeField]
    private HexResourceData[] hexResourceTypes;





    private void Start()
    {
        AssignResources();
        UpdateManager.Instance.OnMonthTick += UpdateMonthlyResources;
    }

    private void UpdateMonthlyResources()
    {
        // Start off by going through each settlement and giving it its neighboring hexes the appropriate resources.
        World.ForEachSettlment((s) => { Debug.Log("Month!"); s.CollectResources(RandomManager.Instance); });
    }

    private void AssignResources()
    {
        // FIXME: SUPER INEFFICIANT FOR GARBAGE COLLECTION! AND UGLY
        foreach (Hex hex in World.HexMap.Values)
        {
            hex.HexResourceData = hexResourceTypes[RandomManager.Instance.rand.Next(0, hexResourceTypes.Length)];
        }

    }

    #region Utility Methods

    public void AddResource(string resource, float amount)
    {
        if (World.Resources.ContainsKey(resource))
        {
            World.Resources[resource] += amount;
        }
        else
        {
            World.Resources.Add(resource, amount);
        }
    }

    public float GetResource(string resource)
    {
        if (World.Resources.ContainsKey(resource))
        {
            return World.Resources[resource];
        }
        World.Resources.Add(resource, 0);
        return 0f;
    }

    public bool AttemptSpendResources(string resource, float amount)
    {
        if (World.Resources.ContainsKey(resource) == false)
        {
            World.Resources.Add(resource, 0);
            return false;
        }
        else if (World.Resources[resource] < amount)
        {
            return false;
        }
        else
        {
            World.Resources[resource] -= amount;
            return true;
        }
    }

    #endregion
}
