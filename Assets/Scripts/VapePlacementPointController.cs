using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;

public class VapePlacementPointController : MonoBehaviour
{
    private bool isOccupied = false;
    private GameObject vape;
    public Vector3 spawnOffset = Vector3.up;

    public void PlaceVape(GameObject incomingVape)
    {
        if(!isOccupied)
        {
            vape = incomingVape;
            vape.transform.position = transform.position + spawnOffset;
            isOccupied = true;
        }
    }

    public void RemoveVape()
    {
        if(isOccupied)
        {
            Destroy(vape);
            isOccupied = false;
        }
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }
}
