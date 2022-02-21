using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{

    [SerializeField] private Building building = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text priceText = null;
    [SerializeField] private LayerMask groundMask = new LayerMask();

    private Camera mainCamera;
    private BoxCollider buildingCollider;
    private RTSPlayer player;

    private GameObject buildingPreview;
    private Renderer[] buildingPreviewRenderers;

    private void Start()
    {
        mainCamera = Camera.main;
        buildingCollider = building.GetComponent<BoxCollider>();
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
        PlaceBuilding();
    }

    public void Build()
    {

        if(player.GetGold() < building.GetPrice()) { return; }

        buildingPreview = Instantiate(building.GetBuildingPreview());
        buildingPreviewRenderers = buildingPreview.GetComponentsInChildren<Renderer>();

        buildingPreview.SetActive(false);
    }

    public void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask)) { return; }

        buildingPreview.transform.position = hit.point;

        if (!buildingPreview.activeSelf)
        {
            buildingPreview.SetActive(true);
        }

        Color color = player.CanPlaceBuilding(buildingCollider, hit.point) ? Color.green : Color.red;

        foreach (Renderer renderer in buildingPreviewRenderers)
        {
            renderer.material.SetColor("_BaseColor", color);
            foreach (var material in renderer.materials)
            {
                material.color = color;
            }
        }


    }

    
    public void PlaceBuilding()
    {
        if (buildingPreview == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
            {
                player.CmdTryPlaceBuilding(building.GetId(), hit.point);
            }


            Destroy(buildingPreview);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Destroy(buildingPreview);
        }
    }

    


}
