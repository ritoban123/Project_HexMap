using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseMode { Normal, SettlementPlacement }

public class MouseManager : MonoBehaviour
{
    public float MouseMoveSensitivity = 0.01f;

    private static MouseManager _instance;
    public static MouseManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MouseManager>();
            return _instance;
        }
    }

    public Vector2 MousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }

    public Vector3 MousePosition3
    {
        get
        {
            return Input.mousePosition;
        }
    }

    public MouseMode MouseMode { get; protected set; }
    public void SetMouseMode(int mode)
    {
        MouseMode = (MouseMode)mode;
    }

    

    public event Action<MouseMode, Vector2> OnMouseMove;

    private void Update()
    {
        // Calculate Mouse Delta based off previous OnMouseMove call. Theoretically, the player can move the mouse
        // as far as he/she/it wants without triggering the callback if he/she/it moves the mouse slowly enough
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1 || Mathf.Abs(Input.GetAxis("Mouse Y")) > MouseMoveSensitivity)
        {
            if (OnMouseMove != null)
                OnMouseMove.Invoke(MouseMode, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        }
    }

}
