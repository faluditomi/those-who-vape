using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerDragAndDropController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggingObject;
    private LayerMask groundLayer;
    private InventoryManager inventoryManager;
    private VapeController.VapeType currentVapeType;
    private static InputActions inputActions;
    private bool isRemoveHeld = false;
    public GameObject vapePrefab;

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        if(vapePrefab)
        {
            currentVapeType = vapePrefab.GetComponent<VapeController>().GetVapeType();
        }
        inputActions = new InputActions();
    }

    private void Start()
    {
        inputActions.Enable();
        inputActions.Gameplay.RemoveTower.started += HoldRemoveButton;
        inputActions.Gameplay.RemoveTower.canceled += HoldRemoveButton;
        inputActions.Gameplay.Click.performed += RemoveVapeIfButtonHeld;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inventoryManager.ManipulateInventory(currentVapeType, -1))
        {
            Vector3 spawnPoint = new Vector3(transform.position.x, 5, transform.position.z);
            draggingObject = Instantiate(vapePrefab, spawnPoint, Quaternion.identity);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPosition = new Vector3(hit.point.x, 5, hit.point.z);
            draggingObject.transform.position = newPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if(hit.collider.CompareTag("VapeZone"))
            {
                VapePlacementPointController vapePlacementPointController = hit.collider.gameObject.GetComponent<VapePlacementPointController>();
                if(!vapePlacementPointController.IsOccupied())
                {
                    vapePlacementPointController.PlaceVape(draggingObject);
                    return;
                }
            }
            inventoryManager.ManipulateInventory(currentVapeType, 1);
            Destroy(draggingObject);
        }
    }

    private void HoldRemoveButton(InputAction.CallbackContext context)
    {
        if(context.action.phase is InputActionPhase.Started)
        {
            isRemoveHeld = true;
        }
        else if(context.action.phase is InputActionPhase.Canceled)
        {
            isRemoveHeld = false;
        }
    }

    private void RemoveVapeIfButtonHeld(InputAction.CallbackContext context)
    {
        if(isRemoveHeld && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)
        && hit.collider.CompareTag("VapeZone"))
        {
            VapePlacementPointController vapePlacementPointController = hit.collider.gameObject.GetComponent<VapePlacementPointController>();
            if(vapePlacementPointController.IsOccupied())
            {
                vapePlacementPointController.RemoveVape();
            }
            
        }
    }
}
