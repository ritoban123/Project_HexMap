using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;



public class RandomManager : MonoBehaviour
{
    private static RandomManager _instance;
    public static RandomManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<RandomManager>();
            return _instance;
        }
    }


    // FIXME: For now, the RandomManager is just a wrapper for the Random object, but later it'll handle things like setting the seed and stuff
    public Random rand = new Random(); // TODO: Allow user to specify a seed

}
