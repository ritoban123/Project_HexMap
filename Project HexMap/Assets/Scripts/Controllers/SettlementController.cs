using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementController : MonoBehaviour
{
    /*
     * The player clicks a button in the UI, which tells a GameManager that we are in settlement placement mode
     * The Mouse Manager has 2 callbacks - OnMouseMoved and OnMouseClicked, both of which take in the current Game Mode
     * On Mouse Moved, if we're in settlement placement mode-
     *   
     *   Calculate the nearest hex corner and display a transparent cope of the settlement
     *       Raycast from the mouse to a flat quad
     *       Use the X and Z coordinates of the hitpoint to calculate which hex the mouse is on
     *       Each hex should have a list of its hex corners (or at least a Dictionary<Hex, List<Vector3>>)
     *       Using brute force search, calculate the closest hex corner (that is within a minimum distance)
     *   If the hex corner is different from the one we were in previous frame (or we just enetered settlement placement mode) AND hexCorner is not null (no corner was close enough)
     *       Hide the Mouse Cursor (it'll look wierd if we have both a SettlementPlaceholder "kind of" following the mouse on top of the regular mouse
     *       Destroy the old gameobject that was representing the settlement
     *       Instantiate a new gameobject at the hex corner (and store a reference to it) 
     *       Create a SettlementPlaceholder data structure (maybe?) to store the placeholder
     *           This should contain the world position, neighboring hexes
     *   Else
     *       Ensure that the mouse cursor is visible
     *       Destory the old gameobject (if it exists)
     * 
     * On Mouse Clicked, if we're in settlement placement mode - 
     *   If Hex Cursor is null
     *       Make sure the Mouse is displayed
     *       Tell the GameManager to return to normal mode
     *       return;
     *   Create a new Settlement
     *   Instantiate a basic settlement gameobject at the appropriate location
     *   Remove any possible SettlementPlaceholders (gameobjects and data structures)
     *   Store these in 2 dictionaries 
     *       BidirectionalDictionary<Settlement, GameObject> settlementGameObjectMap;
     *       Dictionary<HexCorner, Settlement> World.SettlementMap;
     *       
     */


    public LayerMask MouseCollisionMask;

    public void Start()
    {
        MouseManager.Instance.OnMouseMove += OnMouseMove_SettlementPlacement;
    }


    private void OnMouseMove_SettlementPlacement(MouseMode mode, Vector2 delta)
    {
        // Only if we're in settlement placement mode
        if (mode != MouseMode.SettlementPlacement)
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f, MouseCollisionMask))
        {
            Vector3 hexPos = hit.point.WorldPosToHexCoord(HexMapController.Instance.Layout);
            //Debug.Log(hexPos);
            //HexCoord coord = new HexCoord(Mathf.RoundToInt(hexPos.x), Mathf.RoundToInt(hexPos.y), Mathf.RoundToInt(hexPos.z));
            //GameObject obj = HexMapController.Instance.GetGameObjectForHex(HexMapController.Instance.GetHex(coord.q, coord.r, coord.s));
            //obj.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }



}
