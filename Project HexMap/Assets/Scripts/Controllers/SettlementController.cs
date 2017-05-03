using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementController : MonoBehaviour
{
    private static SettlementController _instance;
    public static SettlementController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SettlementController>();
            return _instance;
        }
    }

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
     *   If Hex Corner is null
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


    [SerializeField]
    private LayerMask mouseCollisionMask;
    [SerializeField]
    private GameObject settlementPlaceholderPrefab;
    [SerializeField]
    private GameObject basicSettlementPrefab; // TODO: What about different kinds of base settlements

    public void Start()
    {
        MouseManager.Instance.OnMouseMove += OnMouseMove_SettlementPlacement;
        MouseManager.Instance.OnMouseModeChanged += OnMouseModeChanges_SettlementPlacement;
        MouseManager.Instance.OnLeftMouseReleased += OnLeftMouseReleased_SettlementPlacement;
    }

    private void OnMouseModeChanges_SettlementPlacement(MouseMode m)
    {
        if (m != MouseMode.SettlementPlacement)
            return;

        MouseManager.Instance.HideMouse();
    }

    protected HexCorner lastClosest = null;
    protected GameObject settlementPlaceholderGO;
    protected SettlementPlaceholder settlementPlaceholder;

    private void OnMouseMove_SettlementPlacement(MouseMode mode, Vector2 delta)
    {
        // Only if we're in settlement placement mode
        if (mode != MouseMode.SettlementPlacement)
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f, mouseCollisionMask))
        {
            Vector3 hexPos = HexCoord.WorldPositionToQRS(HexMapController.Instance.Layout, hit.point);
            HexCoord coord = hexPos.RoundHex();
            if (HexMapController.Instance.InRange(coord) == false)
            {
                MouseManager.Instance.UnhideMouse();
                return; // The mouse is not over a hex
            }
            MouseManager.Instance.HideMouse();
            Hex h = HexMapController.Instance.GetHex(coord);
            HexCorner[] corners = h.Corners;

            HexCorner closest = null;
            float bestSqrDist = Mathf.Infinity;

            for (int i = 0; i < corners.Length; i++)
            {
                float sqrDist = Vector3.SqrMagnitude(hit.point - corners[i].WorldPosition);
                if (sqrDist <= bestSqrDist || closest == null)
                {
                    closest = corners[i];
                    bestSqrDist = sqrDist;
                }
            }

            if (closest == null)
                return;

            if (lastClosest != closest)
            {
                lastClosest = closest;
                if (settlementPlaceholderGO != null)
                {
                    Destroy(settlementPlaceholderGO);
                    settlementPlaceholder = null;
                }
                settlementPlaceholder = new SettlementPlaceholder(closest);
                settlementPlaceholderGO = Instantiate(settlementPlaceholderPrefab, closest.WorldPosition, Quaternion.identity, this.transform);
            }


        }

    }

    public BidirectionalDictionary<Settlement, GameObject> SettlementGameObjectMap = new BidirectionalDictionary<Settlement, GameObject>();

    private void OnLeftMouseReleased_SettlementPlacement(MouseMode mode, Vector2 mousePos)
    {
        if (mode != MouseMode.SettlementPlacement)
            return;
        MouseManager.Instance.MouseMode = MouseMode.Normal;
        // No idea how this would happen. As soon as you move you're mouse off of the button, we immediately find the closest hex corner.
        if (settlementPlaceholder == null)
        {
            MouseManager.Instance.MouseMode = MouseMode.Normal;
            return;
        }
        Destroy(settlementPlaceholderGO);
        if (settlementPlaceholder.IsPositionValid(HexMapController.Instance.World) == false)
        {
            Debug.Log("This is not a valid settlement position");
            return;
        }
        Settlement settlement = new Settlement(settlementPlaceholder);
        if (settlement == null)
        {
            // TODO: What if the player needs some minimum resources to build a settlment
            Debug.Log("The settlment creation process did not work");
            return;
        }        //Debug.Log("OnLeftMouseReleased_SettlementPlacement");
        HexMapController.Instance.World.AddSettlement(settlement);
        GameObject GO = Instantiate(basicSettlementPrefab, settlementPlaceholder.HexCorner.WorldPosition, Quaternion.identity, this.transform) as GameObject;
        SettlementGameObjectMap.Add(settlement, GO);
        settlementPlaceholder = null;
    }


}
