using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Building building = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text priceText = null;
    [SerializeField] private LayerMask groundMask = new LayerMask();

    private Camera mainCamera;
    private RTSPlayer player;

    private GameObject buildingPreview;
    private Renderer buildingPreviewRenderer;

    private void Start()
    {
        mainCamera = Camera.main;

        iconImage.sprite = building.GetIcon();
        priceText.text = building.GetPrice().ToString();
    }

    private void Update()
    {
        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (buildingPreview == null) { return; }


        UpdateBuildingPreview();
        //PlaceBuilding();
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        // Replace this method with a public method to be called on a button click if any errors with the OnPointerDown interface...

       if(eventData.button != PointerEventData.InputButton.Left) { return; }

        Debug.Log("Selected Button!");
        buildingPreview = Instantiate(building.GetBuildingPreview());
        buildingPreviewRenderer = buildingPreview.GetComponentInChildren<Renderer>();

        buildingPreview.SetActive(false);
    }

    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask)) { return; }

        buildingPreview.transform.position = hit.point;

        if (!buildingPreview.activeSelf)
        {
            buildingPreview.SetActive(true);
        }

        //Color color = player.CanPlaceBuilding(buildingCollider, hit.point) ? Color.green : Color.red;

        //buildingPreviewRenderer.material.SetColor("_BaseColor", color);

    }

    /*
    public void PlaceBuilding()
    {
        if (buildingPreviewInstance == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                player.CmdTryToPlaceBuilding(building.GetBuildingId(), hit.point);
            }

            Destroy(buildingPreviewInstance);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Destroy(buildingPreviewInstance);
        }
    }

    */


}
