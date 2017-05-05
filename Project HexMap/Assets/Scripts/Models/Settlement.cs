using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    public HexCorner HexCorner;
    public Dictionary<string, float> Resources { get; protected set; }

    private Dictionary<string, bool> DirtyBits = new Dictionary<string, bool>();



    public Settlement(SettlementPlaceholder placeholder)
    {
        HexCorner = placeholder.HexCorner;

        Resources = new Dictionary<string, float>();
    }

    public void CollectResources(RandomManager rand)
    {
        foreach (Hex h in HexCorner.Neighbors)
        {
            for (int i = 0; i < h.HexResourceData.HexResourcesPerMonth.Length; i++)
            {
                HexResourceData.HexResource hr = h.HexResourceData.HexResourcesPerMonth[i];
                if (rand.rand.NextDouble() < hr.CollectProbability)
                {
                    AddResource(hr.Resource, hr.AmountPerMonth);
                }
            }
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
        SetDirty("Resources");
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

    public bool GetDirty(string key)
    {
        return DirtyBits[key];
    }

    public void SetDirty(string key)
    {
        DirtyBits[key] = true;
    }

    // FIXME: What if there are multiple things that need to update?
    public void SetClean(string key)
    {
        DirtyBits[key] = false;
    }

    #endregion

}
