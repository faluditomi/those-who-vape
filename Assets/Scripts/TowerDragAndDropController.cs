using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragAndDropController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject draggingObject;
    private LayerMask groundLayer;
    private InventoryManager inventoryManager;
    public GameObject vapePrefab;

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 spawnPoint = new Vector3(transform.position.x, 5, transform.position.z);
        draggingObject = Instantiate(vapePrefab, spawnPoint, Quaternion.identity);
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
                VapeController.VapeType currentVapeType = draggingObject.GetComponent<VapeController>().GetVapeType();
                hit.collider.gameObject.GetComponent<VapePlacementPointController>().PlaceVape(draggingObject);
                if(inventoryManager.GetNumberOfType(currentVapeType) > 0)
                {
                    inventoryManager.ManipulateInventory(currentVapeType, 1);
                    //TODO: make ui controller and manipulate the counters
                }
            }
            else
            {
                Destroy(draggingObject);
            }
        }
    }
}
