using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Camera mainCamera;
    private List<Unit> selectedUnits = new List<Unit>();

    private void Start()
    {
        mainCamera = Camera.main;

    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Start selecting units with draggable area
            DeselectUnits();
        }  
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            return;
        }
        if(!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

        if(!unit.hasAuthority) { return; }

        selectedUnits.Add(unit);

        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }
    }

    private void DeselectUnits()
    {
        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }
        selectedUnits.Clear();
    }


}
