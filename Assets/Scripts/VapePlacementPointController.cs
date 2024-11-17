using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class VapePlacementPointController : MonoBehaviour
{
    private bool isOccupied = false;
    private AudioManager audioManager;
    private AudioSource audioSource;
    private GameObject vape;
    public Vector3 spawnOffset = Vector3.up;
    public Material hoverMaterial;
    public List<Material> ogMaterials = new List<Material>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void PlaceVape(GameObject incomingVape)
    {
        if(!isOccupied)
        {
            Unselect();
            audioManager.Play("tower_place", audioSource);
            vape = incomingVape;
            vape.transform.position = transform.position + spawnOffset;
            isOccupied = true;
        }
    }

    public void RemoveVape()
    {
        if(isOccupied)
        {
            audioManager.Play("tower_remove", audioSource);
            Destroy(vape);
            isOccupied = false;
        }
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void Select()
    {
        foreach(MeshRenderer meshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = hoverMaterial;
        }
    }

    public void Unselect()
    {
        Transform modelTransform = transform.GetChild(0);
        
        for(int i = 0; i < modelTransform.childCount; i++)
        {
            modelTransform.GetChild(i).GetComponent<MeshRenderer>().material = ogMaterials[i];
        }
    }
}
