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

    public Dictionary<String, float> Resources { get; protected set; }

    [Header("Initialization of Resources")]
    [SerializeField]
    private HexResourceData[] hexResourceTypes;
    [Header("Resource Update Ticks")]
    [SerializeField]
    private float TimePerTick = 10f; //in seconds


    private Random rand = new Random(); // TODO: Allow user to specify a seed
    private Timer MonthlyTickTimer = null;

    private void Awake()
    {
        Resources = new Dictionary<string, float>();
    }
    private void Start()
    {
        AssignResources();
        MonthlyTickTimer = new Timer(TimePerTick, null, null);
    }
    private void AssignResources()
    {
        // FIXME: SUPER INEFFICIANT FOR GARBAGE COLLECTION! AND UGLY

        foreach (Hex hex in HexMapController.Instance.World.HexMap.Values)
        {
            hex.HexResourceData = hexResourceTypes[rand.Next(0,hexResourceTypes.Length)];
        }

    }

    #region Utility Methods

    public void AddResource(string resource, float amount)
    {
        if (Resources.ContainsKey(resource))
        {
            Resources[resource] += amount;
        }
        else
        {
            Resources.Add(resource, amount);
        }
    }

    public float GetResource(string resource)
    {
        if (Resources.ContainsKey(resource))
        {
            return Resources[resource];
        }
        Resources.Add(resource, 0);
        return 0f;
    }

    public bool AttemptSpendResources(string resource, float amount)
    {
        if (Resources.ContainsKey(resource) == false)
        {
            Resources.Add(resource, 0);
            return false;
        }
        else if (Resources[resource] < amount)
        {
            return false;
        }
        else
        {
            Resources[resource] -= amount;
            return true;
        }
    }

    #endregion
}
