using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettlmentInfoDisplay : MonoBehaviour
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

    private Settlement selectedSettlement;

    private void OnLeftMouseReleased_EnterUI(MouseMode m, Vector2 pos)
    {
        if(m != MouseMode.Normal && m != MouseMode.UI)
        {   
            return;
        }

        MouseManager.Instance.MouseMode = MouseMode.UI;


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f, SettlementMask))
        {
            // HACK: We are assuming that the collider is ALWAYS on the child   
            selectedSettlement = SettlementController.Instance.SettlementGameObjectMap[hit.collider.transform.parent.gameObject];
            gameObject.transform.position = MouseManager.Instance.MousePosition;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        ResourceText.text = selectedSettlement.Resources.ResourceDictionaryToText();
        gameObject.SetActive(true);
        selectedSettlement.SetClean("Resources");
    }

    private void Update()
    {
        if (selectedSettlement == null)
            return;

        if (selectedSettlement.GetDirty("Resources"))
        {
            UpdateUI();
        }
    }

    public void CloseResourcesUI()
    {
        gameObject.SetActive(false);
        selectedSettlement = null;
    }
}
