using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private TextMeshProUGUI mixedBerryCounter;
    private TextMeshProUGUI lushIceCounter;
    private TextMeshProUGUI heisenbergCounter;
    private InventoryManager inventoryManager;
    private GameObject pauseState;

    private void Awake()
    {
        inventoryManager = GameObject.FindAnyObjectByType<InventoryManager>();
        mixedBerryCounter = transform.Find("MixedBerryUI").Find("MixedBerryCounter").GetComponent<TextMeshProUGUI>();
        mixedBerryCounter.text = "x " + inventoryManager.GetNumberOfType(VapeController.VapeType.MixedBerry);
        lushIceCounter = transform.Find("LushIceUI").Find("LushIceCounter").GetComponent<TextMeshProUGUI>();
        lushIceCounter.text = "x " + inventoryManager.GetNumberOfType(VapeController.VapeType.LushIce);
        heisenbergCounter = transform.Find("HeisenbergUI").Find("HeisenbergCounter").GetComponent<TextMeshProUGUI>();
        heisenbergCounter.text = "x " + inventoryManager.GetNumberOfType(VapeController.VapeType.Heisenberg);
        pauseState = GameObject.Find("Pause State");
    }

    public void SetPauseState(bool state)
    {
        pauseState.SetActive(state);
    }

    public bool GetPauseState()
    {
        return pauseState.activeInHierarchy;
    }

    public void Restart()
    {
        ManipulateCounter(VapeController.VapeType.MixedBerry, 0);
        ManipulateCounter(VapeController.VapeType.LushIce, 0);
        ManipulateCounter(VapeController.VapeType.Heisenberg, 0);
    }

    public void ManipulateCounter(VapeController.VapeType vapeType, int quantity)
    {
        switch(vapeType)
        {
            case VapeController.VapeType.MixedBerry:
                mixedBerryCounter.text = "x " + quantity;
                break;

            case VapeController.VapeType.LushIce:
                lushIceCounter.text = "x " + quantity;
                break;

            case VapeController.VapeType.Heisenberg:
                heisenbergCounter.text = "x " + quantity;
                break;
        }
    }
}
