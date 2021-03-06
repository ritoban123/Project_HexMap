﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera Camera;
    public float MovementSpeed = 100f; // FIXME: Put this into a keyboard manager
    public float KeyboardSensitivity = 3f; // FIXME: Put this into a keyboard manager

    public float ZoomSpeed = 10000f; // FIXME: Move this into the MouseManager
    public int EdgeBuffer = Mathf.RoundToInt(Screen.height * 0.05f);

    private void Awake()
    {
    }


    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != null)
        {
            if (current.GetComponent<InputField>() != null)
                return;
        }

        Camera.transform.Translate(
        Input.GetAxis("Horizontal") * MovementSpeed * KeyboardSensitivity * Time.deltaTime,
        0,
        Input.GetAxis("Vertical") * MovementSpeed * KeyboardSensitivity * Time.deltaTime,
        Space.World);

        Camera.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime, Space.Self);

        // ALERT: Not using MouseMove callback because this should be called even though the mouse doesn't move 

        if (MouseManager.Instance.MouseMode == MouseMode.Normal && EventSystem.current.IsPointerOverGameObject() == false)
        {

            Vector2 mousePos = MouseManager.Instance.MousePosition;
            if (mousePos.x < EdgeBuffer)
                Camera.transform.Translate(Vector3.left * MovementSpeed * (EdgeBuffer - mousePos.x) * Time.deltaTime, Space.World);
            if (mousePos.y < EdgeBuffer)
                Camera.transform.Translate(Vector3.back * MovementSpeed * (EdgeBuffer - mousePos.y) * Time.deltaTime, Space.World);
            if (mousePos.y > Screen.height - EdgeBuffer)
                Camera.transform.Translate(Vector3.forward * MovementSpeed * -((Screen.height - EdgeBuffer) - mousePos.y) * Time.deltaTime, Space.World);
            if (mousePos.x > Screen.width - EdgeBuffer)
                Camera.transform.Translate(Vector3.right * MovementSpeed * -((Screen.width - EdgeBuffer) - mousePos.x) * Time.deltaTime, Space.World);
        }
    }

}
