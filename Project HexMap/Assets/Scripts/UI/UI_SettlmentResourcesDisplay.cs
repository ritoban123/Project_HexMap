using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettlmentResourcesDisplay : MonoBehaviour
{
    /*
     * If the mouse is clicked on a settlement
     *  Display a dialog
     *  Find the text in that dialog
     *  Set the text to the Resources dictionary of the settlement
     */

    public LayerMask SettlementMask;
    public Text ResourceText;

    private void Awake()
    {
        MouseManager.Instance.OnLeftMouseReleased += OnLeftMouseReleased_EnterUI;
        gameObject.SetActive(false);
    }

    private void OnLeftMouseReleased_EnterUI(MouseMode m, Vector2 pos)
    {
        if(m != MouseMode.Normal)
        {   
            return;
        }

        MouseManager.Instance.MouseMode = MouseMode.UI;


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f, SettlementMask))
        {
            // HACK: We are assuming that the collider is ALWAYS on the child   
            Settlement s = SettlementController.Instance.SettlementGameObjectMap[hit.collider.transform.parent.gameObject];
            ResourceText.text = s.Resources.ResourceDictionaryToText();
            gameObject.SetActive(true); 
        }
    }
}
