using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public MouseMode MouseMode
    {
        get
        {
            return mouseMode;
        }

        set
        {
            mouseMode = value;
            if (OnMouseModeChanged != null)
                OnMouseModeChanged.Invoke(mouseMode);
        }
    }

    private MouseMode mouseMode;

    /// <summary>
    /// THIS IS ONLY FOR THE UI
    /// </summary>
    /// <param name="mode"></param>
    public void SetMouseMode(int mode)
    {
        MouseMode = (MouseMode)mode;
    }

    

    /// <summary>
    /// Called when the mouse moves. MouseMode is the current mouse mode. Vector2 is the mouse DELTA, not Position
    /// </summary>
    public event Action<MouseMode, Vector2> OnMouseMove;
    /// <summary>
    /// Called after the current mouse mode is changed. MouseMode is the new mouse mode 
    /// </summary>
    public event Action<MouseMode> OnMouseModeChanged;
    /// <summary>
    /// Called when the LeftMouseButton is released. Vector
    /// </summary>
    public event Action<MouseMode, Vector2> OnLeftMouseReleased;

    private void Start()
    {
        // ALERT: This could cause problems later on. We may not always want  to unlock the mouse when the mode changes.
        OnMouseModeChanged += UnlockMouse;
    }

    private void Update()
    {
        // Calculate Mouse Delta based off previous OnMouseMove call. Theoretically, the player can move the mouse
        // as far as he/she/it wants without triggering the callback if he/she/it moves the mouse slowly enough
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > MouseMoveSensitivity || Mathf.Abs(Input.GetAxis("Mouse Y")) > MouseMoveSensitivity)
        {
            if (OnMouseMove != null)
                OnMouseMove.Invoke(MouseMode, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
        }
        if(Input.GetMouseButtonUp(0) == true && EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (OnLeftMouseReleased != null)
                OnLeftMouseReleased.Invoke(mouseMode, MousePosition);
        }
    }

    public void HideMouse()
    {
        //Debug.Log("HideMouse");
        Cursor.visible = false;
    }

    private void UnlockMouse(MouseMode m)
    { 
        // HACK for the OnMouseModeChanged Callback

        UnhideMouse();
    }

    public void UnhideMouse()
    {
        //Debug.Log("Unlock Mouse");
        Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

}
