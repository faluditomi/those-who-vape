using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private UIController uiController;
    private int numberOfMixedBerry = 0;
    private int numberOfLushIce = 0;
    private int numberOfHeisenberg = 0;

    private void Awake()
    {
        uiController = GameObject.FindAnyObjectByType<UIController>();
    }

    public void Restart()
    {
        numberOfMixedBerry = 99;
        numberOfLushIce = 99;
        numberOfHeisenberg = 99;
    }

    public bool ManipulateInventory(VapeController.VapeType vapeType, int quantity)
    {
        if(GetNumberOfType(vapeType) + quantity < 0)
        {
            return false;
        }

        switch(vapeType)
        {
            case VapeController.VapeType.MixedBerry:
                numberOfMixedBerry += quantity;
                break;

            case VapeController.VapeType.LushIce:
                numberOfLushIce += quantity;
                break;

            case VapeController.VapeType.Heisenberg:
                numberOfHeisenberg += quantity;
                break;
        }
        
        uiController.ManipulateCounter(vapeType, GetNumberOfType(vapeType));
        return true;
    }

    public int GetNumberOfType(VapeController.VapeType vapeType)
    {
        switch(vapeType)
        {
            case VapeController.VapeType.MixedBerry:
                return numberOfMixedBerry;

            case VapeController.VapeType.LushIce:
                return numberOfLushIce;

            case VapeController.VapeType.Heisenberg:
                return numberOfHeisenberg;

            default:
                return -1;
        }
    }
}
