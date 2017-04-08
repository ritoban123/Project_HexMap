using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Vector2 MousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }
        
}
