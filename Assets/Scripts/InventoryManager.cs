using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int numberOfMixedBerry = 0;
    private int numberOfLushIce = 0;
    private int numberOfHeisenberg = 0;

    public void ManipulateInventory(VapeController.VapeType vapeType, int quantity)
    {
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
