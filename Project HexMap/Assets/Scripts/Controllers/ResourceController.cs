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
    /* Each Hex needs to provide some resources
     * The Total Resources can be stored in a dictionary (in the Resource Controller)
     * The Resources given each tick (each month) can also be stored in a dictionary
     */

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

    [Header("Initialization of Resources")]
    [SerializeField]
    private HexResourceData[] hexResourceTypes;

    public Dictionary<String, float> Resources { get; protected set; }

    private Random rand = new Random(); // TODO: Allow user to specify a seed

    private void Awake()
    {
        Resources = new Dictionary<string, float>();
    }

    private void Start()
    {
        AssignResources();
    }

    private void AssignResources()
    {
        foreach (Hex hex in HexMapController.Instance.hexGameObjectMap.Keys)
        {
            hex.HexResourceData = hexResourceTypes[Mathf.FloorToInt((float)rand.NextDouble() * hexResourceTypes.Length)];
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
